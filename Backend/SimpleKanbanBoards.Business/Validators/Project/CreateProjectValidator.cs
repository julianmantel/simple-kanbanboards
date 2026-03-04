using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Org.BouncyCastle.Security;
using SimpleKanbanBoards.Business.Models.Project;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;

namespace SimpleKanbanBoards.Business.Validators.Project
{
    public class CreateProjectValidator : AbstractValidator<CreateProjectModel>
    {
        private int _maxTitleLength = ProjectValidatonRules.MaxTitleLength;
        private int _maxDescriptionLength = ProjectValidatonRules.MaxDescriptionLength;

        public CreateProjectValidator()
        {
        RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Project title is required.")
                .MaximumLength(_maxTitleLength)
                    .WithMessage($"Project name must not exceed {_maxTitleLength} characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                    .WithMessage("Project description is required.")
                .MaximumLength(_maxDescriptionLength)
                    .WithMessage($"Project description must not exceed {_maxDescriptionLength} characters.");

            RuleFor(x => x.MaxDevs)
                .GreaterThan(0)
                    .WithMessage("Max developers must be greater than zero.");
        }
    }
}
