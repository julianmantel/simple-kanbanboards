using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SimpleKanbanBoards.Business.Models.Task;

namespace SimpleKanbanBoards.Business.Validators.Task
{
    public class UpdateTaskValidator : AbstractValidator<UpdateTaskModel>
    {
        private int _titleMaxLength = TaskValidationRules.TITLE_MAX_LENGTH;
        private int _descriptionMaxLength = TaskValidationRules.DESCRIPTION_MAX_LENGTH;
        private int _serviceClassMaxLength = TaskValidationRules.SERVICECLASS_MAX_LENGTH;

        public UpdateTaskValidator()
        {
            RuleFor(task => task.Title)
                .NotEmpty()
                    .WithMessage("Task title is required.")
                .MaximumLength(_titleMaxLength)
                    .WithMessage($"Task title must not exceed {_titleMaxLength} characters.");
            RuleFor(task => task.Description)
                .NotEmpty()
                    .WithMessage("Task description is required.")
                .MaximumLength(_descriptionMaxLength)
                    .WithMessage($"Task description must not exceed {_descriptionMaxLength} characters.");
            RuleFor(task => task.ServiceClass)
                .NotEmpty()
                    .WithMessage("Task service class is required.")
                .MaximumLength(_serviceClassMaxLength)
                    .WithMessage($"Task service class must not exceed {_serviceClassMaxLength} characters.");
            RuleFor(task => task.Priority)
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Priority must be greater than zero.");
        }
    }
}
