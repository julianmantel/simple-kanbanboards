using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleKanbanBoards.Business.Models;
using SimpleKanbanBoards.Business.Models.Project;
using SimpleKanbanBoards.Business.Service.IService;

namespace SimpleKanbanBoards.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Project Manager")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = await _projectService.GetProjectByIDAsync(id);
            if (project == null) return NotFound(ApiResult<string>.Failure(new List<string> { "Project not found" }));

            return Ok(ApiResult<ProjectModel>.Success(project));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectModel project)
        {
            await _projectService.CreateProjectAsync(project);
            return Ok(ApiResult<string>.Success("Project created successfully"));
        }

        [HttpPost("dev")]
        public async Task<IActionResult> AddDevToProject([FromBody] ProjectUserModel request)
        {
            await _projectService.AddDevToProjectAsync(request);
            return Ok(ApiResult<string>.Success("Developer added to project successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            await _projectService.DeleteProjectAsync(id);

            return Ok(ApiResult<string>.Success("Project deleted successfully"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectModel request)
        {
            await _projectService.UpdateProjectAsync(request);

            return Ok(ApiResult<string>.Success("Project updated successfully"));
        }
    }
}
