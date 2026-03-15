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
    public class UpdateTaskValidatorTests
    {
        private readonly UpdateTaskValidator _validator = new();
        private int MaxTitleLength => TaskValidationRules.TITLE_MAX_LENGTH;
        private int MaxDescriptionLength => TaskValidationRules.DESCRIPTION_MAX_LENGTH;
        private int MaxServiceClassLength => TaskValidationRules.SERVICECLASS_MAX_LENGTH;

        [Fact]
        public void ShouldHaveError_WhenTitleIsEmpty()
        {
            var model = new UpdateTaskModel { Id = 1, Title = "", Description = "Description", ServiceClass = "Expedite", Priority = 1, IdBoardColumn = 1 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Title)
                  .WithErrorMessage("Task title is required.");
        }

        [Fact]
        public void ShouldHaveError_WhenTitleExceedsMaxLength()
        {
            var model = new UpdateTaskModel
            {
                Id = 1,
                Title = new string('T', MaxTitleLength + 1),
                Description = "Description",
                ServiceClass = "Expedite",
                Priority = 1,
                IdBoardColumn = 1
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void ShouldHaveError_WhenDescriptionIsEmpty()
        {
            var model = new UpdateTaskModel { Id = 1, Title = "Title", Description = "", ServiceClass = "Expedite", Priority = 1, IdBoardColumn = 1 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage("Task description is required.");
        }

        [Fact]
        public void ShouldHaveError_WhenDescriptionExceedsMaxLength()
        {
            var model = new UpdateTaskModel
            {
                Id = 1,
                Title = "Valid Title",
                Description = new string('D', MaxDescriptionLength + 1),
                ServiceClass = "Expedite",
                Priority = 1,
                IdBoardColumn = 1
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage($"Task description must not exceed {MaxDescriptionLength} characters.");
        }

        [Fact]
        public void ShouldHaveError_WhenServiceClassIsEmpty()
        {
            var model = new UpdateTaskModel { Id = 1, Title = "Title", Description = "Description", ServiceClass = "", Priority = 1, IdBoardColumn = 1 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ServiceClass)
                  .WithErrorMessage("Task service class is required.");
        }

        [Fact]
        public void ShouldHaveError_WhenServiceClassExceedsMaxLength()
        {
            var model = new UpdateTaskModel
            {
                Id = 1,
                Title = "Valid Title",
                Description = "Valid Description",
                ServiceClass = new string('S', MaxServiceClassLength + 1),
                Priority = 1,
                IdBoardColumn = 1
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ServiceClass)
                  .WithErrorMessage($"Task service class must not exceed {MaxServiceClassLength} characters.");
        }

        [Fact]
        public void ShouldHaveError_WhenPriorityIsNegative()
        {
            var model = new UpdateTaskModel { Id = 1, Title = "Title", Description = "Description", ServiceClass = "Expedite", Priority = -1, IdBoardColumn = 1 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Priority)
                  .WithErrorMessage("Priority must be greater than zero.");
        }

        [Fact]
        public void ShouldNotHaveErrors_WhenModelIsValid()
        {
            var model = new UpdateTaskModel { Id = 1, Title = "My Task", Description = "A description", ServiceClass = "Expedite", Priority = 5, IdBoardColumn = 1 };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
