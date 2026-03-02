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
    public class ResetPassRequestValidatorTests
    {
        private readonly ResetPassRequestValidator _validator = new();

        [Fact]
        public void ShouldHaveError_WhenEmailIsEmpty()
        {
            var model = new ResetPassRequestModel { Email = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("Email is required.");
        }

        [Fact]
        public void ShouldHaveError_WhenEmailIsInvalidFormat()
        {
            var model = new ResetPassRequestModel { Email = "not-an-email" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("A valid email is required.");
        }

        [Fact]
        public void ShouldNotHaveErrors_WhenEmailIsValid()
        {
            var model = new ResetPassRequestModel { Email = "user@example.com" };
            _validator.TestValidate(model).ShouldNotHaveAnyValidationErrors();
        }
    }
}
