using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SimpleKanbanBoards.Business.Exceptions;
using SimpleKanbanBoards.Business.Models.Task;
using SimpleKanbanBoards.Tests.Integration.Common;
using SimpleKanbanBoards.Tests.Integration.Helpers;
using SimpleKanbanBoards.Tests.Integration.Helpers.ApiHelpers;
using Xunit;

namespace SimpleKanbanBoards.Tests.Integration.Controllers
{
    public class TasksControllerTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public TasksControllerTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateAuthenticatedClient("Developer");
        }

        [Fact]
        public async Task GET_Task_WhenTaskExists_ShouldReturn200()
        {
            var task = new TaskModel { Id = 1, Title = "Implement login", Description = "Implement user login functionality" };
            _factory.TaskServiceMock.Setup(s => s.GetTaskById(It.IsAny<int>())).ReturnsAsync(task);

            var response = await _client.GetAsync("/api/tasks/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await ResponseHelper.GetApiResultAsync<TaskModel>(response);
            CheckResponse.Succeeded(body);
            body.Result.Title.Should().Be("Implement login");
        }

        [Fact]
        public async Task GET_Task_WhenTaskDoesNotExist_ShouldReturn404WithFailureMessage()
        {
            _factory.TaskServiceMock.Setup(s => s.GetTaskById(It.IsAny<int>())).ReturnsAsync((TaskModel?)null);
            var response = await _client.GetAsync("/api/tasks/404");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task POST_Task_WithValidPayload_ShouldReturn200()
        {
            var createTaskModel = new CreateTaskModel
            {
                Title = "Implement registration",
                Description = "Implement user registration functionality",
                IdBoardColumn = 1,
                CreatedAt = DateTime.UtcNow,
                Priority = 1,
                ServiceClass = "Development"
            };

            var response = await _client.PostAsJsonAsync("/api/tasks", createTaskModel);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await ResponseHelper.GetApiResultAsync<string>(response);
            CheckResponse.Succeeded(body);
            body.Result.Should().Be("Task created successfully");
        }

        [Fact]
        public async Task POST_Task_WithInvalidPayload_ShouldReturn400WithValidationErrors()
        {
            var createTaskModel = new CreateTaskModel
            {
                Title = "",
                Description = "Implement user registration functionality",
                IdBoardColumn = 1,
                CreatedAt = DateTime.UtcNow,
                Priority = 1,
                ServiceClass = "Development"
            };

            var response = await _client.PostAsJsonAsync("/api/tasks", createTaskModel);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var body = await ResponseHelper.GetApiResultAsync<string>(response);
            CheckResponse.Failure(body);
        }

        [Fact]
        public async Task DELETE_Task_WhenTaskExists_ShouldReturn200()
        {
            _factory.TaskServiceMock.Setup(s => s.DeleteTask(1)).Returns(Task.CompletedTask);
            var response = await _client.DeleteAsync("/api/tasks/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await ResponseHelper.GetApiResultAsync<string>(response);
            CheckResponse.Succeeded(body);
            body.Result.Should().Be("Task deleted successfully");
        }

        [Fact]
        public async Task DELETE_Task_WhenTaskDoesNotExist_ShouldReturn404()
        {
            _factory.TaskServiceMock.Setup(s => s.DeleteTask(It.IsAny<int>()))
                                                 .ThrowsAsync(new NotFoundException("Task not found"))
                ;
            var response = await _client.DeleteAsync("/api/tasks/404");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PUT_Task_WithValidPayload_ShouldReturn200()
        {
            var updateTaskModel = new UpdateTaskModel
            {
                Id = 1,
                Title = "Implement login - Updated",
                Description = "Implement user login functionality with OAuth",
                IdBoardColumn = 1,
                CompletedAt = null,
                Priority = 1,
                ServiceClass = "Development"
            };
            var response = await _client.PutAsJsonAsync("/api/tasks", updateTaskModel);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await ResponseHelper.GetApiResultAsync<string>(response);
            CheckResponse.Succeeded(body);
            body.Result.Should().Be("Task updated successfully");
        }

        [Fact]
        public async Task PUT_Task_WithInvalidPayload_ShouldReturn400WithValidationErrors()
        {
            var updateTaskModel = new UpdateTaskModel
            {
                Id = 1,
                Title = "",
                Description = "Implement user login functionality with OAuth",
                IdBoardColumn = 1,
                CompletedAt = null,
                Priority = 1,
                ServiceClass = "Development"
            };
            var response = await _client.PutAsJsonAsync("/api/tasks", updateTaskModel);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await ResponseHelper.GetApiResultAsync<string>(response);
            CheckResponse.Failure(body);
        }

        [Fact]
        public async Task POST_AssignTaskToUser_WithValidPayload_ShouldReturn200()
        {
            var userTaskModel = new UserTaskModel
            {
                IdUser = 1,
                IdTask = 1
            };

            var response = await _client.PostAsJsonAsync("/api/tasks/assign", userTaskModel);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await ResponseHelper.GetApiResultAsync<string>(response);
            CheckResponse.Succeeded(body);
            body.Result.Should().Be("Task assigned to user successfully");
        }

        [Fact]
        public async Task POST_AssignTaskToUser_WhenTaskAlreadyAssigned_ShouldReturn409()
        {
            var userTaskModel = new UserTaskModel
            {
                IdUser = 1,
                IdTask = 1
            };

            _factory.TaskServiceMock
                                .Setup(s => s.AddUserTask(It.IsAny<UserTaskModel>()))
                                .ThrowsAsync(new ConflictException("Task is already assigned to the user"));

            var response = await _client.PostAsJsonAsync("/api/tasks/assign", userTaskModel);

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task GET_TasksByBoardColumnId_WhenTasksExist_ShouldReturn200WithTasks()
        {
            var tasks = new List<TaskModel>
            {
                new TaskModel { Id = 1, Title = "Task 1", Description = "Description 1" },
                new TaskModel { Id = 2, Title = "Task 2", Description = "Description 2" }
            };
            _factory.TaskServiceMock.Setup(s => s.GetTasksByColumnIdAsync(It.IsAny<int>())).ReturnsAsync(tasks);

            var response = await _client.GetAsync("/api/tasks/boardcolumn/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await ResponseHelper.GetApiResultAsync<IEnumerable<TaskModel>>(response);
            CheckResponse.Succeeded(body);
            body.Result.Should().HaveCount(2);
        }
    }
}
