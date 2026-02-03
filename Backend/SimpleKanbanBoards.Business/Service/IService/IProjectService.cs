using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleKanbanBoards.Business.Models.Project;

namespace SimpleKanbanBoards.Business.Service.IService
{
    public interface IProjectService
    {
        Task<ProjectModel> GetProjectByIDAsync(int projectId);
        Task CreateProjectAsync(CreateProjectModel project);
        Task AddDevToProjectAsync(ProjectUserModel request);
        Task DeleteProjectAsync(int projectId);
        Task UpdateProjectAsync(UpdateProjectModel project);
    }
}
