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
    public class UpdateProjectValidator : AbstractValidator<UpdateProjectModel>
    {
        private readonly IProjectRepository _projectRepository;
        private int maxTitleLength = ProjectValidatonRules.MaxTitleLength;
        private int maxDescriptionLength = ProjectValidatonRules.MaxDescriptionLength;

        public UpdateProjectValidator(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Project title is required.")
                .MaximumLength(maxTitleLength).WithMessage($"Project title must not exceed {maxTitleLength} characters.")
                .MustAsync(IsProjectNameUnique).WithMessage("A project with that name already exists.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Project description is required.")
                .MaximumLength(maxDescriptionLength).WithMessage($"Project description must not exceed {maxDescriptionLength} characters.");

            RuleFor(x => x.MaxDevs)
                .GreaterThan(0).WithMessage("Max developers must be greater than zero.");
        }

        private async Task<bool> IsProjectNameUnique(string title, CancellationToken cancellationToken)
        {
            return await _projectRepository.Exist(p => p.Title == title); ;
        }
    }
}
