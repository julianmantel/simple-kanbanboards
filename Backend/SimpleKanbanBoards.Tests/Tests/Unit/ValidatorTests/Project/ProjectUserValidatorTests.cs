using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Moq;
using SimpleKanbanBoards.Business.Models.Project;
using SimpleKanbanBoards.Business.Validators.Project;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;
using Xunit;

namespace SimpleKanbanBoards.Tests.Unit.ValidatorTests.Project
{
    public class ProjectUserValidatorTests
    {
        private readonly ProjectUserValidator _validator = new();

        [Fact]
        public void ShouldHaveError_WhenIdDevIsZero()
        {
            var model = new ProjectUserModel { IdDev = 0, IdProject = 1 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.IdDev)
                  .WithErrorMessage("Developer ID is required.");
        }

        [Fact]
        public void ShouldHaveError_WhenIdProjectIsZero()
        {
            var model = new ProjectUserModel { IdDev = 1, IdProject = 0 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.IdProject)
                  .WithErrorMessage("Project ID is required.");
        }

        [Fact]
        public void ShouldNotHaveErrors_WhenBothIdsAreValid()
        {
            var model = new ProjectUserModel { IdDev = 1, IdProject = 1 };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
