using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using SimpleKanbanBoards.Business.Exceptions;
using SimpleKanbanBoards.Business.Models.Email;
using SimpleKanbanBoards.Business.Models.Role;
using SimpleKanbanBoards.Business.Models.User;
using SimpleKanbanBoards.Business.Service.IService;
using SimpleKanbanBoards.Business.Templates.ITemplate;
using SimpleKanbanBoards.Business.Utils;
using SimpleKanbanBoards.DataAccess.Models;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;
using Task = System.Threading.Tasks.Task;

namespace SimpleKanbanBoards.Business.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IResetPasswordRepository _resetPasswordRepository;
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateBuilder _emailTemplateBuilder;

        private const int EXPIRE_RESET_TOKEN_MINUTES = 5;

        public UserService(
            IUserRepository userRepository, 
            IConfiguration configuration, 
            IResetPasswordRepository resetPasswordRepository, 
            IEmailService emailService,
            IEmailTemplateBuilder emailTemplateBuilder)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _resetPasswordRepository = resetPasswordRepository;
            _emailService = emailService;
            _emailTemplateBuilder = emailTemplateBuilder;
        }

        public async Task<UserModel> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetFirstOrDefault(u => u.IdUser == id, u => u.IdRols);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            return new UserModel
            {
                Id = user.IdUser,
                UserName = user.Username,
                Roles = user.IdRols.Select(r => new RoleModel
                {
                    Id = r.IdRol,
                    Name = r.RolName
                }).ToList()
            };
        }

        public async Task CreateUserAsync(CreateUserModel newUser)
        {
            var userExist = await _userRepository.Exist(u => u.Username == newUser.UserName || u.Email == newUser.Email);
            if (userExist)
            {
                throw new ConflictException("User with the same username or email already exists.");
            }

            AuthUtil.CreatePasswordHash(newUser.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new User
            {
                Username = newUser.UserName,
                Email = newUser.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IdRols = newUser.Roles.Select(roleId => new Role { IdRol = roleId }).ToList()
            };

            await _userRepository.AddAsync(user);

            return;
        }

        public async Task<string> LoginAsync(LoginRequestModel request)
        {
            var user = await _userRepository.GetFirstOrDefault(u => u.Username == request.UserName, u => u.IdRols);
            if (user == null)
            {
                throw new UnauthorizedException("Invalid username or password.");
            }

            bool validPassword = AuthUtil.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);
            if (!validPassword)
            {
                throw new UnauthorizedException("Invalid username or password.");
            }

            var userModel = new UserModel
            {
                Id = user.IdUser,
                UserName = user.Username,
                Roles = user.IdRols.Select(r => new RoleModel { Id = r.IdRol, Name = r.RolName }).ToList()
            };
            var jwtToken = JwtUtil.GenerateToken(userModel, _configuration);

            return jwtToken;
        }

        public async Task ResetPasswordAsync(ResetPassRequestModel request)
        {
            var user = await _userRepository.GetFirstOrDefault(u => u.Email == request.Email);
            if (user == null)
            {
                throw new NotFoundException("User with the provided email does not exist.");
            }

            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(16));

            PasswordResetToken passwordReset = new PasswordResetToken
            {
                ResetToken = token,
                ResetTokenExpire = DateTime.UtcNow.AddMinutes(EXPIRE_RESET_TOKEN_MINUTES),
                IdUser = user.IdUser
            };
            await _resetPasswordRepository.AddAsync(passwordReset);


            var tokenWeb = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            string url = $"{_configuration["EmailSettings:AppUrl"]}/ChangePassword?token={tokenWeb}";

            EmailRequestModel emailRequest = new EmailRequestModel
            {
                To = request.Email,
                Subject = "Change password",
                Message = _emailTemplateBuilder.BuildResetPasswordEmail(url)
            };

            await _emailService.SendEmailAsync(emailRequest);
        }

        public async Task ChangePasswordAsync(ChangePasswordModel request)
        {
            var decodedTokenBytes = WebEncoders.Base64UrlDecode(request.Token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

            var resetToken = await _resetPasswordRepository.GetFirstOrDefault(r => r.ResetToken == decodedToken);

            if (resetToken == null || resetToken.ResetTokenExpire < DateTime.UtcNow)
            {
                throw new UnauthorizedException("Invalid or expired password reset token.");
            }

            var user = await _userRepository.GetFirstOrDefault(u => u.IdUser == resetToken.IdUser);
            AuthUtil.CreatePasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _userRepository.Update(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetFirstOrDefault(u => u.IdUser == id);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            _userRepository.Remove(user);
        }
    }
}
