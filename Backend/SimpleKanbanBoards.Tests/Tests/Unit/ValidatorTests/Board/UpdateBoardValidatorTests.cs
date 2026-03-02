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
    }
}
