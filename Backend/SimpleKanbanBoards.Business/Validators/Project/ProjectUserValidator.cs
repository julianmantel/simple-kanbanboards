using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SimpleKanbanBoards.Business.Models.Project;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;

namespace SimpleKanbanBoards.Business.Validators.Project
{
    public class ProjectUserValidator : AbstractValidator<ProjectUserModel>
    {
        public ProjectUserValidator()
        {
            RuleFor(x => x.IdDev)
                .NotEmpty()
                    .WithMessage("Developer ID is required.");


            RuleFor(x => x.IdProject)
                .NotEmpty()
                    .WithMessage("Project ID is required.");
        }
    }
}
