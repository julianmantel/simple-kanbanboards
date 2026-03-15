using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleKanbanBoards.Business.Models;
using SimpleKanbanBoards.Business.Models.Task;
using SimpleKanbanBoards.Business.Service.IService;

namespace SimpleKanbanBoards.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Project Manager, Developer")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("boardcolumn/{columnId}")]
        public async Task<IActionResult> GetTasksByBoardColumnId(int columnId)
        {
            var tasks = await _taskService.GetTasksByColumnIdAsync(columnId);
            return Ok(ApiResult<IEnumerable<TaskModel>>.Success(tasks));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(ApiResult<TaskModel>.Success(task));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskModel createTaskModel)
        {
            await _taskService.CreateTask(createTaskModel);
            return Ok(ApiResult<string>.Success("Task created successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await _taskService.DeleteTask(id);
            return Ok(ApiResult<string>.Success("Task deleted successfully"));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskModel updateTaskModel)
        {
            await _taskService.UpdateTask(updateTaskModel);
            return Ok(ApiResult<string>.Success("Task updated successfully"));
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignTaskToUser([FromBody] UserTaskModel userTask)
        {
            await _taskService.AddUserTask(userTask);
            return Ok(ApiResult<string>.Success("Task assigned to user successfully"));
        }
    }
}
