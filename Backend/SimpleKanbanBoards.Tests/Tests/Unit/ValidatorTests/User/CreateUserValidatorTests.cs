using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Moq;
using SimpleKanbanBoards.Business.Models.User;
using SimpleKanbanBoards.Business.Validators.User;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;
using Xunit;

namespace SimpleKanbanBoards.Tests.Unit.ValidatorTests.User
{
    public class CreateUserValidatorTests
    {
        private readonly CreateUserValidator _validator;

        public CreateUserValidatorTests()
        {
            var repoMock = new Mock<IUserRepository>();
            _validator = new CreateUserValidator(repoMock.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData("ab")] // too short (< 3)
        public void ShouldHaveError_WhenUserNameIsInvalid(string userName)
        {
            var model = new CreateUserModel { UserName = userName, Password = "Pass1A", Email = "a@b.com", Roles = new List<int>() };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.UserName);
        }

        [Fact]
        public void ShouldHaveError_WhenUserNameExceedsMaxLength()
        {
            var model = new CreateUserModel
            {
                UserName = new string('u', UserValidationRules.MaxUserNameLength + 1),
                Password = "Pass1A",
                Email = "a@b.com",
                Roles = new List<int>()
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.UserName);
        }

        [Fact]
        public void ShouldHaveError_WhenPasswordHasNoUppercase()
        {
            var model = new CreateUserModel { UserName = "valid", Password = "nouppercase1", Email = "a@b.com", Roles = new List<int>() };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Password)
                  .WithErrorMessage("Password must contain at least one uppercase letter.");
        }

        [Fact]
        public void ShouldHaveError_WhenPasswordHasNoDigit()
        {
            var model = new CreateUserModel { UserName = "valid", Password = "NoDigitPass", Email = "a@b.com", Roles = new List<int>() };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Password)
                  .WithErrorMessage("Password must contain at least one digit.");
        }

        [Fact]
        public void ShouldHaveError_WhenEmailIsInvalid()
        {
            var model = new CreateUserModel { UserName = "valid", Password = "Pass1A", Email = "not-an-email", Roles = new List<int>() };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("A valid email is required.");
        }

        [Fact]
        public void ShouldNotHaveErrors_WhenModelIsValid()
        {
            var model = new CreateUserModel { UserName = "johndoe", Password = "Pass1A", Email = "john@email.com", Roles = new List<int> { 1 } };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
