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
    public class CreateBoardValidatorTests
    {
        private readonly CreateBoardValidator _validator = new();
        private int BoardNameMinLength => BoardValidationRules.BOARDNAME_MIN_LENGTH;
        private int BoardNameMaxLength => BoardValidationRules.BOARDNAME_MAX_LENGTH;
        private int BoardDescriptionMaxLength => BoardValidationRules.BOARDDESCRIPTION_MAX_LENGTH;


        [Fact]
        public void ShouldHaveError_WhenNameIsEmpty()
        {
            var model = new CreateBoardModel { Name = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Board name is required.");
        }

        [Fact]
        public void ShouldHaveError_WhenNameIsTooShort()
        {
            var model = new CreateBoardModel { Name = "Ab" }; // 2 chars < minLength=3
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage($"Board name must be at least {BoardNameMinLength} characters long.");
        }

        [Fact]
        public void ShouldHaveError_WhenNameIsTooLong()
        {
            var model = new CreateBoardModel { Name = new string('A', BoardNameMaxLength + 1) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage($"Board name must not exceed {BoardNameMaxLength} characters.");
        }

        [Fact]
        public void ShouldHaveError_WhenDescriptionExceedsMaxLength()
        {
            var model = new CreateBoardModel
            {
                Name = "Valid Name",
                Description = new string('X', BoardDescriptionMaxLength + 1)
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void ShouldNotHaveErrors_WhenModelIsValid()
        {
            var model = new CreateBoardModel { Name = "Sprint 1", Description = "First sprint" };

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void ShouldNotHaveError_WhenDescriptionIsEmpty()
        {
            var model = new CreateBoardModel { Name = "Sprint 1", Description = "" };

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void ShouldNotHaveError_WhenDescriptionIsNull()
        {
            var model = new CreateBoardModel { Name = "Sprint 1", Description = null };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }
    }
}
