using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Moq;
using SimpleKanbanBoards.Business.Exceptions;
using SimpleKanbanBoards.Business.Models.Email;
using SimpleKanbanBoards.Business.Models.User;
using SimpleKanbanBoards.Business.Service;
using SimpleKanbanBoards.Business.Service.IService;
using SimpleKanbanBoards.Business.Templates.ITemplate;
using SimpleKanbanBoards.Business.Utils;
using SimpleKanbanBoards.DataAccess.Models;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;
using System.Linq.Expressions;
using System.Text;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace SimpleKanbanBoards.Tests.Unit.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IResetPasswordRepository> _resetPassRepoMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly Mock<IEmailTemplateBuilder> _templateBuilderMock;
    private readonly IConfiguration _configuration;
    private readonly UserService _sut;

    public UserServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _resetPassRepoMock = new Mock<IResetPasswordRepository>();
        _emailServiceMock = new Mock<IEmailService>();
        _templateBuilderMock = new Mock<IEmailTemplateBuilder>();

        var settings = new Dictionary<string, string>
        {
            { "JWT:SecretKey", "super-secret-key-1234567890-padding-for-length" },
            { "JWT:Issuer", "TestIssuer" },
            { "JWT:Audience", "TestAudience" },
            { "JWT:ExpirationMinutes", "30" },
            { "EmailSettings:AppUrl", "http://localhost:3000" }
        };
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings!)
            .Build();

        _sut = new UserService(
            _userRepoMock.Object,
            _configuration,
            _resetPassRepoMock.Object,
            _emailServiceMock.Object,
            _templateBuilderMock.Object
        );
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenUserExists_ShouldReturnMappedModel()
    {
        var user = new User
        {
            IdUser = 1,
            Username = "Dager",
            IdRols = new List<Role> { new Role { IdRol = 2, RolName = "Developer" } }
        };
        _userRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(user);

        var result = await _sut.GetUserByIdAsync(1);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.UserName.Should().Be("Dager");
        result.Roles.Should().HaveCount(1);
        result.Roles!.First().Name.Should().Be("Developer");
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenUserNotFound_ShouldThrowNotFoundException()
    {

        _userRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync((User?)null);

        var act = async () => await _sut.GetUserByIdAsync(99);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage("User not found.");
    }

    [Fact]
    public async Task CreateUserAsync_WhenUserIsNew_ShouldHashPasswordAndAddUser()
    {
        var model = new CreateUserModel
        {
            UserName = "CavaniAndateDeBoca",
            Password = "ContraseniaMuySegura",
            Email = "idk@email.com",
            Roles = new List<int> { 1 }
        };
        _userRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<User, bool>>>()))
                     .ReturnsAsync(false);

        await _sut.CreateUserAsync(model);

        _userRepoMock.Verify(r => r.AddAsync(It.Is<User>(u =>
            u.Username == model.UserName &&
            u.Email == model.Email &&
            u.PasswordHash != null &&
            u.PasswordHash.Length > 0 &&
            u.PasswordSalt != null
        )), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_WhenUsernameOrEmailAlreadyExists_ShouldThrowConflictException()
    {
        var model = new CreateUserModel { UserName = "Descartes", Password = "1637", 
                                          Email = "descartado@email.com", Roles = new List<int>() };
        _userRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<User, bool>>>()))
                     .ReturnsAsync(true);

        var act = async () => await _sut.CreateUserAsync(model);

        await act.Should().ThrowAsync<ConflictException>()
                 .WithMessage("User with the same username or email already exists.");
        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnJwtToken()
    {
        AuthUtil.CreatePasswordHash("1234", out var hash, out var salt);
        var user = new User
        {
            IdUser = 1,
            Username = "ElBananero",
            PasswordHash = hash,
            PasswordSalt = salt,
            IdRols = new List<Role> { new Role { IdRol = 1, RolName = "Project Manager" } }
        };
        _userRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(user);

        var request = new LoginRequestModel { UserName = "ElBananero", Password = "1234" };

        var token = await _sut.LoginAsync(request);

        token.Should().NotBeNullOrEmpty();
        token.Split('.').Should().HaveCount(3, because: "a JWT has 3 parts separated by dots");
    }

    [Fact]
    public async Task LoginAsync_WhenUserNotFound_ShouldThrowUnauthorizedException()
    {
        _userRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync((User?)null);

        var request = new LoginRequestModel { UserName = "NeuronasDeRiquelme", Password = "2" };

        var act = async () => await _sut.LoginAsync(request);

        await act.Should().ThrowAsync<UnauthorizedException>()
                 .WithMessage("Invalid username or password.");
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIsWrong_ShouldThrowUnauthorizedException()
    {
        AuthUtil.CreatePasswordHash("1234", out var hash, out var salt);
        var user = new User { IdUser = 1, Username = "AbueloConAlzheimer", PasswordHash = hash, PasswordSalt = salt, IdRols = new List<Role>() };
        _userRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Expression<Func<User, object>>[]>()))
            .ReturnsAsync(user);

        var request = new LoginRequestModel { UserName = "AbueloConAlzheimer", Password = "123" };

        var act = async () => await _sut.LoginAsync(request);

        await act.Should().ThrowAsync<UnauthorizedException>()
                 .WithMessage("Invalid username or password.");
    }

    [Fact]
    public async Task ResetPasswordAsync_WhenEmailExists_ShouldCreateTokenAndSendEmail()
    {
        var user = new User { IdUser = 1, Username = "GordonFreeman", Email = "MrGordonFreeman3pls@email.com" };
        _userRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);
        _templateBuilderMock.Setup(t => t.BuildResetPasswordEmail(It.IsAny<string>())).Returns("<html>reset</html>");
        _emailServiceMock.Setup(e => e.SendEmailAsync(It.IsAny<EmailRequestModel>())).ReturnsAsync("Sent Mail");

        var request = new ResetPassRequestModel { Email = "MrGordonFreeman3pls@email.com" };

        await _sut.ResetPasswordAsync(request);

        _resetPassRepoMock.Verify(r => r.AddAsync(It.Is<PasswordResetToken>(t =>
            t.IdUser == user.IdUser &&
            t.ResetToken != null &&
            t.ResetTokenExpire > DateTime.UtcNow
        )), Times.Once);
        _emailServiceMock.Verify(e => e.SendEmailAsync(It.Is<EmailRequestModel>(m =>
            m.To == request.Email &&
            m.Subject == "Change password"
        )), Times.Once);
    }

    [Fact]
    public async Task ResetPasswordAsync_WhenEmailNotFound_ShouldThrowNotFoundException()
    {
        _userRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((User?)null);

        var request = new ResetPassRequestModel { Email = "unknown@email.com" };

        var act = async () => await _sut.ResetPasswordAsync(request);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage("User with the provided email does not exist.");
    }

    [Fact]
    public async Task ChangePasswordAsync_WithValidToken_ShouldUpdatePasswordHash()
    {
        var rawToken = "ABCDEF1234567890ABCDEF1234567890";
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(rawToken));

        var resetToken = new PasswordResetToken
        {
            ResetToken = rawToken,
            ResetTokenExpire = DateTime.UtcNow.AddMinutes(5),
            IdUser = 1
        };
        var user = new User { IdUser = 1, PasswordHash = new byte[64], PasswordSalt = new byte[128] };

        _resetPassRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<PasswordResetToken, bool>>>()))
            .ReturnsAsync(resetToken);
        _userRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);

        var request = new ChangePasswordModel { Token = encodedToken, NewPassword = "NewPass1" };

        await _sut.ChangePasswordAsync(request);

        _userRepoMock.Verify(r => r.Update(It.Is<User>(u =>
            u.IdUser == 1 &&
            u.PasswordHash != null &&
            u.PasswordSalt != null
        )), Times.Once);
    }

    [Fact]
    public async Task ChangePasswordAsync_WithExpiredToken_ShouldThrowUnauthorizedException()
    {
        var rawToken = "EXPIREDTOKEN12345678901234567890";
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(rawToken));

        var resetToken = new PasswordResetToken
        {
            ResetToken = rawToken,
            ResetTokenExpire = DateTime.UtcNow.AddMinutes(-10),
            IdUser = 1
        };
        _resetPassRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<PasswordResetToken, bool>>>()))
            .ReturnsAsync(resetToken);

        var request = new ChangePasswordModel { Token = encodedToken, NewPassword = "Therian" };

        var act = async () => await _sut.ChangePasswordAsync(request);

        await act.Should().ThrowAsync<UnauthorizedException>()
                 .WithMessage("Invalid or expired password reset token.");
    }

    [Fact]
    public async Task ChangePasswordAsync_WhenTokenNotFound_ShouldThrowUnauthorizedException()
    {
        var rawToken = "NOTEXIST12345678901234567890ABCD";
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(rawToken));

        _resetPassRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<PasswordResetToken, bool>>>()))
            .ReturnsAsync((PasswordResetToken?)null);

        var request = new ChangePasswordModel { Token = encodedToken, NewPassword = "GTAVI" };

        var act = async () => await _sut.ChangePasswordAsync(request);

        await act.Should().ThrowAsync<UnauthorizedException>()
                 .WithMessage("Invalid or expired password reset token.");
    }

    [Fact]
    public async Task DeleteUserAsync_WhenUserExists_ShouldCallRemove()
    {
        var user = new User { IdUser = 1 };
        _userRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);

        await _sut.DeleteUserAsync(1);

        _userRepoMock.Verify(r => r.Remove(user), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_WhenUserNotFound_ShouldThrowNotFoundException()
    {
        _userRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((User?)null);

        var act = async () => await _sut.DeleteUserAsync(99);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage("User not found.");
    }
}
