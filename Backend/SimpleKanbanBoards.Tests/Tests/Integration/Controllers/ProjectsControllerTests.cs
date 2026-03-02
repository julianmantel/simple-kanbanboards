using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SimpleKanbanBoards.Business.Models;
using SimpleKanbanBoards.Business.Models.Project;
using SimpleKanbanBoards.Tests.Integration.Common;
using SimpleKanbanBoards.Tests.Integration.Helpers;
using SimpleKanbanBoards.Tests.Integration.Helpers.ApiHelpers;
using Xunit;

namespace SimpleKanbanBoards.Tests.Integration.Controllers
{
    public class ProjectsControllerTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly TestWebApplicationFactory _factory;

        public ProjectsControllerTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateAuthenticatedClient("Project Manager");
        }

        [Fact]
        public async Task GET_Project_WhenProjectExists_ShouldReturn200()
        {
            var project = new ProjectModel { Id = 1, Title = "Project Speedrun Any%", Description = "Apuren este proyecto loco" };
            _factory.ProjectServiceMock.Setup(s => s.GetProjectByIDAsync(1)).ReturnsAsync(project);

            var response = await _client.GetAsync("/api/projects/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await ResponseHelper.GetApiResultAsync<ProjectModel>(response);
            CheckResponse.Succeeded(body);
            body.Result.Title.Should().Be("Project Speedrun Any%");
        }

        [Fact]
        public async Task GET_Project_WhenProjectDoesNotExist_ShouldReturn404WithFailureMessage()
        {
            _factory.ProjectServiceMock.Setup(s => s.GetProjectByIDAsync(404)).ReturnsAsync((ProjectModel?)null);

            var response = await _client.GetAsync("/api/projects/404");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var body = await ResponseHelper.GetApiResultAsync<string>(response);
            CheckResponse.Failure(body);
            body.Errors.Should().Contain("Project not found");
        }

        [Fact]
        public async Task POST_Project_WithValidPayload_ShouldReturn200()
        {
            _factory.ProjectServiceMock.Setup(s => s.CreateProjectAsync(It.IsAny<CreateProjectModel>()))
                                       .Returns(Task.CompletedTask);

            var payload = new CreateProjectModel { Title = "Escaleras de Penrose", Description = "Infinitos bugs", MaxDevs = 3, AuthorId = 1 };

            var response = await _client.PostAsJsonAsync("/api/projects", payload);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DELETE_Project_ShouldReturn200()
        {
            _factory.ProjectServiceMock.Setup(s => s.DeleteProjectAsync(1)).Returns(Task.CompletedTask);

            var response = await _client.DeleteAsync("/api/projects/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task PUT_Project_WithValidPayload_ShouldReturn200()
        {
            _factory.ProjectServiceMock.Setup(s => s.UpdateProjectAsync(It.IsAny<UpdateProjectModel>()))
                                       .Returns(Task.CompletedTask);

            var payload = new UpdateProjectModel { Id = 1, Title = "Updated", Description = "Desc", MaxDevs = 4 };

            var response = await _client.PutAsJsonAsync("/api/projects/1", payload);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
