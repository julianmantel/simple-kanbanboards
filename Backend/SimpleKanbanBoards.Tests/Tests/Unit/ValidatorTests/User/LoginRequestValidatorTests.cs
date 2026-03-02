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
    public class LoginRequestValidatorTests
    {
        private readonly LoginRequestValidator _validator = new();

        [Fact]
        public void ShouldHaveError_WhenUserNameIsEmpty()
        {
            var model = new LoginRequestModel { UserName = "", Password = "pass" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.UserName)
                  .WithErrorMessage("UserName is required.");
        }

        [Fact]
        public void ShouldHaveError_WhenPasswordIsEmpty()
        {
            var model = new LoginRequestModel { UserName = "johndoe", Password = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Password)
                  .WithErrorMessage("Password is required.");
        }

        [Fact]
        public void ShouldNotHaveErrors_WhenModelIsValid()
        {
            var model = new LoginRequestModel { UserName = "johndoe", Password = "somepass" };
            _validator.TestValidate(model).ShouldNotHaveAnyValidationErrors();
        }
    }
}
