using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.Ocsp;
using SimpleKanbanBoards.Business.Exceptions;
using SimpleKanbanBoards.Business.Models.Board;
using SimpleKanbanBoards.Business.Models.Project;
using SimpleKanbanBoards.Business.Service.IService;
using SimpleKanbanBoards.DataAccess.Models;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;
using Task = System.Threading.Tasks.Task;

namespace SimpleKanbanBoards.Business.Service
{

    public class ProjectService : IProjectService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        public ProjectService(IUserRepository userRepository, IProjectRepository projectRepository)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        public async Task AddDevToProjectAsync(ProjectUserModel request)
        {
            var project = await _projectRepository.GetFirstOrDefault(p => p.IdProject == request.IdProject);
            if(project == null)
            {
                throw new NotFoundException("Project does not exist.");
            }

            var userExist = await _userRepository.Exist(u => u.IdUser == request.IdDev);
            if(!userExist) 
            {
                throw new NotFoundException("User does not exist.");
            }

            var isUserInProject = await _projectRepository.IsUserInProject(request.IdDev);
            if(isUserInProject)
            {
                throw new ConflictException("User is already assigned to this project.");
            }

            var hasNotMaxDevs = _projectRepository.CountUsers(request.IdProject) <= project.MaxDevs;
            if(!hasNotMaxDevs)
            {
                throw new ConflictException("Project has reached the maximum number of developers.");
            }

            var userProject = new UserProject
            {
                IdProject = request.IdProject,
                IdUser = request.IdDev
            };

            await _projectRepository.AddUserProject(userProject);

        }

        public async Task CreateProjectAsync(CreateProjectModel project)
        {
            var projectExist = await _projectRepository.Exist(p => p.Title == project.Title);
            if(projectExist)
            {
                throw new ConflictException("Project with the same title already exists.");
            }

            var projectManager = await _userRepository.GetFirstOrDefault(u => u.IdUser == project.AuthorId);

            var newProject = new Project
            {
                Title = project.Title,
                Description = project.Description,
                MaxDevs = project.MaxDevs
            };

            await _projectRepository.AddAsync(newProject);

            await AddDevToProjectAsync(new ProjectUserModel
            {
                IdDev = projectManager.IdUser,
                IdProject = newProject.IdProject
            });

        }

        public async Task<ProjectModel> GetProjectByIDAsync(int projectId)
        {
            var project = await _projectRepository.GetFirstOrDefault(p => p.IdProject == projectId, p => p.Boards);
            if (project == null)
            {
                throw new NotFoundException("Project does not exist.");
            }

            var projectModel = new ProjectModel
            {
                Id = project.IdProject,
                Title = project.Title,
                Description = project.Description,
                StartDate = project.StartDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
                EndDate = project.EndDate,
                MaxDevs = project.MaxDevs ?? 1,
                Boards = project.Boards.Select(b => new BoardModel
                {
                    Id = b.IdBoard,
                    Name = b.BoardName,
                    Description = b.Description,
                    Created_At = b.CreatedAt ?? DateOnly.FromDateTime(DateTime.UtcNow)
                }).ToList()
            };

            return projectModel;
        }

        public async Task DeleteProjectAsync(int projectId)
        {
            var project = await _projectRepository.GetFirstOrDefault(p => p.IdProject == projectId);
            if (project == null)
            {
                throw new NotFoundException("Project does not exist.");
            }

            _projectRepository.Remove(project);
        }

        public async Task UpdateProjectAsync(UpdateProjectModel project)
        {
            var existingProject = await _projectRepository.GetFirstOrDefault(p => p.IdProject == project.Id);
            if (existingProject == null)
            {
                throw new NotFoundException("Project does not exist.");
            }

            var titleExist = await _projectRepository.Exist(p => p.Title == project.Title);
            if(titleExist && existingProject.Title != project.Title)
            {
                throw new ConflictException("Another project with the same title already exists.");
            }

            existingProject.Title = project.Title;
            existingProject.Description = project.Description;
            existingProject.MaxDevs = project.MaxDevs;

            _projectRepository.Update(existingProject);
        }
    }
}
