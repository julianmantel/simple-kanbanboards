using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using SimpleKanbanBoards.Business.Models.Project;
using SimpleKanbanBoards.Business.Validators.Project;
using Xunit;

namespace SimpleKanbanBoards.Tests.Unit.ValidatorTests.Project
{
    public class UpdateProjectValidatorTests
    {
        private readonly UpdateProjectValidator _validator = new();

        [Fact]
        public void ShouldHaveError_WhenTitleIsEmpty()
        {
            var model = new UpdateProjectModel { Title = "", Description = "Desc", MaxDevs = 2 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void ShouldHaveError_WhenDescriptionIsEmpty()
        {
            var model = new UpdateProjectModel { Title = "Title", Description = "", MaxDevs = 2 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void ShouldHaveError_WhenMaxDevsIsZero()
        {
            var model = new UpdateProjectModel { Title = "Title", Description = "Desc", MaxDevs = 0 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.MaxDevs);
        }

        [Fact]
        public void ShouldNotHaveErrors_WhenModelIsValid()
        {
            var model = new UpdateProjectModel { Id = 1, Title = "Updated", Description = "Updated desc", MaxDevs = 4 };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
