using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SimpleKanbanBoards.Business.Models.Task;

namespace SimpleKanbanBoards.Business.Validators.Task
{
    public class UserTaskValidator : AbstractValidator<UserTaskModel>
    {
        public UserTaskValidator()
        {
            RuleFor(task => task.IdTask)
                .GreaterThan(0)
                    .WithMessage("Task ID must be greater than zero.");
            RuleFor(task => task.IdUser)
                .GreaterThan(0)
                    .WithMessage("User ID must be greater than zero.");
        }
    }
}
