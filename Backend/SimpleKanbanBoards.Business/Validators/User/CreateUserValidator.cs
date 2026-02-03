using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using SimpleKanbanBoards.Business.Models.User;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;

namespace SimpleKanbanBoards.Business.Validators.User
{
    public class CreateUserValidator : AbstractValidator<CreateUserModel>
    {
        private readonly IUserRepository _userRepository;
        private int minUserNameLength = UserValidationRules.MinUserNameLength;
        private int maxUserNameLength = UserValidationRules.MaxUserNameLength;
        private int minPasswordLength = UserValidationRules.MinPasswordLength;

        public CreateUserValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required.")
                .MinimumLength(minUserNameLength).WithMessage($"UserName must be at least {minUserNameLength} characters long.")
                .MaximumLength(maxUserNameLength).WithMessage($"UserName must not exceed {maxUserNameLength} characters.")
                .Must(IsUserNameUnique).WithMessage("Username already exists.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(minPasswordLength).WithMessage($"Password must be at least {minPasswordLength} characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.")
                .Must(IsEmailUnique).WithMessage("Email already exists.");
        }

        private bool IsUserNameUnique(string username)
        {
            var user = _userRepository.GetFirstOrDefault(u => u.Username == username);

            return user == null;
        }

        private bool IsEmailUnique(string email)
        {
            var user = _userRepository.GetFirstOrDefault(u => u.Email == email);

            return user == null;
        }
    }
}
