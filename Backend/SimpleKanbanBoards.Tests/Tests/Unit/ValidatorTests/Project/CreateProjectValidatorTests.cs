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
    public class CreateProjectValidatorTests
    {
        private readonly CreateProjectValidator _validator = new();

        [Fact]
        public void ShouldHaveError_WhenTitleIsEmpty()
        {
            var model = new CreateProjectModel { Title = "", Description = "Desc", MaxDevs = 3 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Title)
                  .WithErrorMessage("Project title is required.");
        }

        [Fact]
        public void ShouldHaveError_WhenTitleExceedsMaxLength()
        {
            var model = new CreateProjectModel
            {
                Title = new string('T', ProjectValidatonRules.MaxTitleLength + 1),
                Description = "Desc",
                MaxDevs = 2
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void ShouldHaveError_WhenDescriptionIsEmpty()
        {
            var model = new CreateProjectModel { Title = "Title", Description = "", MaxDevs = 2 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage("Project description is required.");
        }

        [Fact]
        public void ShouldHaveError_WhenMaxDevsIsZero()
        {
            var model = new CreateProjectModel { Title = "Title", Description = "Desc", MaxDevs = 0 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.MaxDevs)
                  .WithErrorMessage("Max developers must be greater than zero.");
        }

        [Fact]
        public void ShouldHaveError_WhenMaxDevsIsNegative()
        {
            var model = new CreateProjectModel { Title = "Title", Description = "Desc", MaxDevs = -1 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.MaxDevs);
        }

        [Fact]
        public void ShouldNotHaveErrors_WhenModelIsValid()
        {
            var model = new CreateProjectModel { Title = "My Project", Description = "A description", MaxDevs = 5 };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
