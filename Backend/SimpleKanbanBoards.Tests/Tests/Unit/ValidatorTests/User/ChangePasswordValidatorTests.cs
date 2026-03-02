using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using SimpleKanbanBoards.Business.Models.User;
using SimpleKanbanBoards.Business.Validators.User;
using Xunit;

namespace SimpleKanbanBoards.Tests.Unit.ValidatorTests.User
{
    public class ChangePasswordValidatorTests
    {
        private readonly ChangePasswordValidator _validator = new();

        [Fact]
        public void ShouldHaveError_WhenNewPasswordIsEmpty()
        {
            var model = new ChangePasswordModel { NewPassword = "", Token = "sometoken" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.NewPassword)
                  .WithErrorMessage("New Password is required.");
        }

        [Fact]
        public void ShouldHaveError_WhenNewPasswordLacksUppercase()
        {
            var model = new ChangePasswordModel { NewPassword = "nouppercase1", Token = "sometoken" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.NewPassword)
                  .WithErrorMessage("New Password must contain at least one uppercase letter.");
        }

        [Fact]
        public void ShouldHaveError_WhenNewPasswordLacksDigit()
        {
            var model = new ChangePasswordModel { NewPassword = "NoDigitPass", Token = "sometoken" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.NewPassword)
                  .WithErrorMessage("New Password must contain at least one digit.");
        }

        [Fact]
        public void ShouldHaveError_WhenTokenIsEmpty()
        {
            var model = new ChangePasswordModel { NewPassword = "Valid1Pass", Token = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Token)
                  .WithErrorMessage("Token is required.");
        }

        [Fact]
        public void ShouldNotHaveErrors_WhenModelIsValid()
        {
            var model = new ChangePasswordModel { NewPassword = "NewPass1", Token = "validtoken" };
            _validator.TestValidate(model).ShouldNotHaveAnyValidationErrors();
        }
    }
}
