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
    public class CreateBoardColumnValidatorTests
    {
        private readonly CreateBoardColumnValidator _validator = new();

        [Fact]
        public void ShouldHaveError_WhenNameIsEmpty()
        {
            var model = new CreateBoardColumnModel { Name = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Column name is required.");
        }

        [Fact]
        public void ShouldHaveError_WhenNameIsTooLong()
        {
            var model = new CreateBoardColumnModel { Name = new string('K', BoardColumnValidationRules.MaxNameLength + 1) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage($"Column name cannot exceed {BoardColumnValidationRules.MaxNameLength} characters.");
        }

        [Fact]
        public void ShouldHaveError_WhenPositionIsNegative()
        {
            var model = new CreateBoardColumnModel { Name = "Valid Name", Position = -1 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Position)
                  .WithErrorMessage("Position must be a non-negative integer.");
        }

        [Fact]
        public void ShouldHaveError_WhenWipLimitIsNegative()
        {
            var model = new CreateBoardColumnModel { Name = "Valid Name", WipLimit = -1 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.WipLimit)
                  .WithErrorMessage("WIP Limit must be a non-negative integer.");
        }

        [Fact]
        public void ShouldNotHaveError_WhenModelIsValid()
        {
            var model = new CreateBoardColumnModel
            {
                Name = "Valid Name",
                Position = 0,
                WipLimit = 5
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
            result.ShouldNotHaveValidationErrorFor(x => x.Position);
            result.ShouldNotHaveValidationErrorFor(x => x.WipLimit);
        }
    }
}
