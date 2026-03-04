using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using SimpleKanbanBoards.Business.Models.BoardColumn;
using SimpleKanbanBoards.Business.Validators.BoardColumn;
using Xunit;

namespace SimpleKanbanBoards.Tests.Unit.ValidatorTests.BoardColumn
{
    public class UpdateBoardColumnValidatorTests
    {
        private readonly UpdateBoardColumnValidator _validator = new();

        [Fact]
        public void ShouldHaveError_WhenNameIsEmpty()
        {
            var model = new UpdateBoardColumnModel { Name = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Column name is required.");
        }

        [Fact]
        public void ShouldHaveError_WhenNameIsTooLong()
        {
            var model = new UpdateBoardColumnModel { Name = new string('K', BoardColumnValidationRules.MaxNameLength + 1) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage($"Column name cannot exceed {BoardColumnValidationRules.MaxNameLength} characters.");
        }

        [Fact]
        public void ShouldHaveError_WhenPositionIsNegative()
        {
            var model = new UpdateBoardColumnModel { Name = "Valid Name", Position = -1 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Position)
                  .WithErrorMessage("Position must be a non-negative integer.");
        }

        [Fact]
        public void ShouldHaveError_WhenWipLimitIsNegative()
        {
            var model = new UpdateBoardColumnModel { Name = "Valid Name", WipLimit = -1 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.WipLimit)
                  .WithErrorMessage("WIP Limit must be a non-negative integer.");
        }

        [Fact]
        public void ShouldNotHaveError_WhenModelIsValid()
        {
            var model = new UpdateBoardColumnModel
            {
                Name = "Valid Name",
                Position = 0,
                WipLimit = 5
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
