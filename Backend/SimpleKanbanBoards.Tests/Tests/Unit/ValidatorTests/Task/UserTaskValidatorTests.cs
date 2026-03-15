using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using SimpleKanbanBoards.Business.Models.Task;
using SimpleKanbanBoards.Business.Validators.Task;
using Xunit;

namespace SimpleKanbanBoards.Tests.Unit.ValidatorTests.Task
{
    public class UserTaskValidatorTests
    {
        private readonly UserTaskValidator _validator = new();

        [Fact]
        public void ShouldHaveError_WhenIdTaskIsZero()
        {
            var model = new UserTaskModel { IdTask = 0, IdUser = 1 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.IdTask)
                  .WithErrorMessage("Task ID must be greater than zero.");
        }

        [Fact]
        public void ShouldHaveError_WhenIdTaskIsNegative()
        {
            var model = new UserTaskModel { IdTask = -1, IdUser = 1 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.IdTask)
                  .WithErrorMessage("Task ID must be greater than zero.");
        }

        [Fact]
        public void ShouldHaveError_WhenIdUserIsZero()
        {
            var model = new UserTaskModel { IdTask = 1, IdUser = 0 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.IdUser)
                  .WithErrorMessage("User ID must be greater than zero.");
        }

        [Fact]
        public void ShouldHaveError_WhenIdUserIsNegative()
        {
            var model = new UserTaskModel { IdTask = 1, IdUser = -1 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.IdUser)
                  .WithErrorMessage("User ID must be greater than zero.");
        }

        [Fact]
        public void ShouldNotHaveErrors_WhenModelIsValid()
        {
            var model = new UserTaskModel { IdTask = 1, IdUser = 2 };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
