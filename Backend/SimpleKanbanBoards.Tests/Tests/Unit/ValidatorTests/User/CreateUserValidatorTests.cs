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
        private readonly CreateUserValidator _validator = new();
        private int MinUserNameLength => UserValidationRules.USERNAME_MIN_LENGTH;
        private int MaxUserNameLength => UserValidationRules.USERNAME_MAX_LENGTH;

        [Fact]
        public void ShouldHaveError_WhenUserNameIsTooShort()
        {
            var model = new CreateUserModel
            {
                UserName = new string('u', MinUserNameLength - 1),
                Password = "Pass1A",
                Email = "a@b.com"
            };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.UserName);

        }

        [Fact]
        public void ShouldHaveError_WhenUserNameExceedsMaxLength()
        {
            var model = new CreateUserModel
            {
                UserName = new string('u', MaxUserNameLength + 1),
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
