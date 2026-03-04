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
using SimpleKanbanBoards.Business.Models.Board;
using SimpleKanbanBoards.Tests.Integration.Common;
using SimpleKanbanBoards.Tests.Integration.Helpers;
using SimpleKanbanBoards.Tests.Integration.Helpers.ApiHelpers;
using Xunit;

namespace SimpleKanbanBoards.Tests.Integration.Controllers
{
    public class BoardsControllerTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly TestWebApplicationFactory _factory;

        public BoardsControllerTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateAuthenticatedClient("Project Manager");
        }

        [Fact]
        public async Task GET_Board_WhenBoardExists_ShouldReturn200WithBoard()
        {
            var board = new BoardModel { Id = 1, Name = "Sprint RNG", Description = "Depende del azar", ProjectId = 1, Is_Active = true };
            _factory.BoardServiceMock.Setup(s => s.GetBoardByIdAsync(1)).ReturnsAsync(board);

            var response = await _client.GetAsync("/api/boards/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await ResponseHelper.GetApiResultAsync<BoardModel>(response);
            CheckResponse.Succeeded(body);
            body.Result.Id.Should().Be(1);
            body.Result.Name.Should().Be("Sprint RNG");
        }

        [Fact]
        public async Task GET_Board_WhenBoardDoesNotExist_ShouldReturn404()
        {
            _factory.BoardServiceMock.Setup(s => s.GetBoardByIdAsync(999)).ReturnsAsync((BoardModel?)null);

            var response = await _client.GetAsync("/api/boards/999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task POST_Board_WithValidPayload_ShouldReturn200()
        {
            _factory.BoardServiceMock.Setup(s => s.CreateBoardAsync(It.IsAny<BoardModel>()))
                                     .Returns(Task.CompletedTask);

            var payload = new BoardModel { Name = "New Board", Description = "Desc", ProjectId = 1 };

            var response = await _client.PostAsJsonAsync("/api/boards", payload);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await ResponseHelper.GetApiResultAsync<string>(response);
            CheckResponse.Succeeded(body);
        }

        [Fact]
        public async Task DELETE_Board_ShouldReturn200()
        {
            _factory.BoardServiceMock.Setup(s => s.ToggleBoardAsync(1)).Returns(Task.CompletedTask);

            var response = await _client.DeleteAsync("/api/boards/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task PUT_Board_WithValidPayload_ShouldReturn200()
        {
            _factory.BoardServiceMock.Setup(s => s.UpdateBoardAsync(It.IsAny<UpdateBoardModel>()))
                                     .Returns(Task.CompletedTask);

            var payload = new UpdateBoardModel { Id = 1, Name = "Boss Phase 2", Description = "Updated desc" };

            var response = await _client.PutAsJsonAsync("/api/boards/1", payload);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
