using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using SimpleKanbanBoards.Business.Models.Board;
using SimpleKanbanBoards.Business.Validators.Board;
using Xunit;

namespace SimpleKanbanBoards.Tests.Unit.ValidatorTests.Board
{
    public class UpdateBoardValidatorTests
    {
        private readonly UpdateBoardValidator _validator = new();
        private int BoardNameMinLength => BoardValidationRules.BOARDNAME_MIN_LENGTH;
        private int BoardNameMaxLength => BoardValidationRules.BOARDNAME_MAX_LENGTH;
        private int BoardDescriptionMaxLength => BoardValidationRules.BOARDDESCRIPTION_MAX_LENGTH;

        [Fact]
        public void ShouldHaveError_WhenNameIsEmpty()
        {
            var result = _validator.TestValidate(new UpdateBoardModel { Name = "" });
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void ShouldNotHaveErrors_WhenModelIsValid()
        {
            var model = new UpdateBoardModel { Id = 1, Name = "Updated Board", Description = "Updated desc" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void ShouldHaveError_WhenNameIsTooShort()
        {
            var result = _validator.TestValidate(new UpdateBoardModel { Name = "Ab" });
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage($"Board name must be at least {BoardNameMinLength} characters long.");
        }

        [Fact]
        public void ShouldHaveError_WhenNameIsTooLong()
        {
            var result = _validator.TestValidate(new UpdateBoardModel { Name = new string('A', BoardNameMaxLength + 1) });
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage($"Board name must not exceed {BoardNameMaxLength} characters.");
        }

        [Fact]
        public void ShouldHaveError_WhenDescriptionExceedsMaxLength()
        {
            var result = _validator.TestValidate(new UpdateBoardModel
            {
                Name = "Valid Name",
                Description = new string('X', BoardDescriptionMaxLength + 1)
            });
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage($"Board description must not exceed {BoardDescriptionMaxLength} characters.");
        }

    }
}
