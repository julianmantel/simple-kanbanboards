using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SimpleKanbanBoards.Business.Models.Task;

namespace SimpleKanbanBoards.Business.Validators.Task
{
    public class CreateTaskValidator : AbstractValidator<CreateTaskModel>
    {
        private int _maxTitleLength = TaskValidationRules.TITLE_MAX_LENGTH;
        private int _maxDescriptionLength = TaskValidationRules.DESCRIPTION_MAX_LENGTH;
        private int _maxServiceClassLength = TaskValidationRules.SERVICECLASS_MAX_LENGTH;

        public CreateTaskValidator()
        {
            RuleFor(task => task.Title)
                .NotEmpty()
                    .WithMessage("Task title is required.")
                .MaximumLength(_maxTitleLength)
                    .WithMessage($"Task title must not exceed {_maxTitleLength} characters.");
            RuleFor(task => task.Description)
                .NotEmpty()
                    .WithMessage("Task description is required.")
                .MaximumLength(_maxDescriptionLength)
                    .WithMessage($"Task description must not exceed {_maxDescriptionLength} characters.");
            RuleFor(task => task.ServiceClass)
                .NotEmpty()
                    .WithMessage("Task service class is required.")
                .MaximumLength(_maxServiceClassLength)
                    .WithMessage($"Task service class must not exceed {_maxServiceClassLength} characters.");
            RuleFor(task => task.Priority)
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Priority must be greater than zero.");
                
        }
    }
}
