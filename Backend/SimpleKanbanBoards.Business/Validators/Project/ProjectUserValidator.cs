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
        private readonly IProjectRepository _projectRepository;

        public ProjectUserValidator(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;

            RuleFor(x => x.IdDev)
                .NotEmpty().WithMessage("Developer ID is required.")
                .MustAsync(IsNotUserInProject).WithMessage("User is already assigned to this project");


            RuleFor(x => x.IdProject)
                .NotEmpty().WithMessage("Project ID is required.")
                .MustAsync(ProjectExists).WithMessage("Project does not exist.")
                .MustAsync(HasNotMaxDevs).WithMessage("Project has reached the maximum number of developers.");
        }

        private async Task<bool> ProjectExists(int projectId, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetFirstOrDefault(p => p.IdProject == projectId);
            return project != null;
        }

        private async Task<bool> IsNotUserInProject(int user, CancellationToken cancellationToken)
        {
            return !(await _projectRepository.IsUserInProject(user));
        }

        private async Task<bool> HasNotMaxDevs(int projectID, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetFirstOrDefault(p => p.IdProject == projectID);

            return _projectRepository.CountUsers(projectID) <= project.MaxDevs;
        }
    }
}
