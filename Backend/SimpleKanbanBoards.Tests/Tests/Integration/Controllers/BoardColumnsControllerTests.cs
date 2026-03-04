using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SimpleKanbanBoards.Business.Models.BoardColumn;
using SimpleKanbanBoards.Tests.Integration.Common;
using SimpleKanbanBoards.Tests.Integration.Helpers;
using SimpleKanbanBoards.Tests.Integration.Helpers.ApiHelpers;
using Xunit;

namespace SimpleKanbanBoards.Tests.Integration.Controllers
{
    public class BoardColumnsControllerTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly TestWebApplicationFactory _factory;

        public BoardColumnsControllerTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateAuthenticatedClient("Project Manager");
        }

        [Fact]
        public async Task GET_BoardColumn_WhenBoardColumnExists_ShouldReturn200WithBoardColumn()
        {
            var boardColumn = new BoardColumnModel { Id = 1, Name = "To Do", Position = 0, WipLimit = 5, IdBoard = 1 };
            _factory.BoardColumnServiceMock.Setup(s => s.GetBoardColumnByIdAsync(1))
                                    .ReturnsAsync(boardColumn);

            var response = await _client.GetAsync("/api/boardcolumns/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await ResponseHelper.GetApiResultAsync<BoardColumnModel>(response);
            CheckResponse.Succeeded(body);
            body.Result.Id.Should().Be(1);
            body.Result.Name.Should().Be("To Do");
        }

        [Fact]
        public async Task GET_BoardColumn_WhenBoardColumnDoesNotExist_ShouldReturn404()
        {
            _factory.BoardColumnServiceMock.Setup(s => s.GetBoardColumnByIdAsync(999)).ReturnsAsync((BoardColumnModel?)null);

            var response = await _client.GetAsync("/api/boardcolumns/999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task POST_BoardColumn_WithValidPayload_ShouldReturn200()
        {
            _factory.BoardColumnServiceMock.Setup(s => s.CreateBoardColumnAsync(It.IsAny<CreateBoardColumnModel>()))
                                         .Returns(Task.CompletedTask);
            var newColumn = new CreateBoardColumnModel { Name = "In Progress", Position = 1, WipLimit = 3, IdBoard = 1 };
            var response = await _client.PostAsJsonAsync("/api/boardcolumns", newColumn);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await ResponseHelper.GetApiResultAsync<string>(response);
            CheckResponse.Succeeded(body);
            body.Result.Should().Be("Board column created successfully");
        }

        [Fact]
        public async Task DELETE_BoardColumn_ShouldReturn200()
        {
            _factory.BoardColumnServiceMock.Setup(s => s.DeleteBoardColumnAsync(1)).Returns(Task.CompletedTask);
            var response = await _client.DeleteAsync("/api/boardcolumns/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await ResponseHelper.GetApiResultAsync<string>(response);
            CheckResponse.Succeeded(body);
            body.Result.Should().Be("Board column deleted successfully");
        }

        [Fact]
        public async Task PUT_BoardColumn_WithValidPayload_ShouldReturn200()
        {
            _factory.BoardColumnServiceMock.Setup(s => s.UpdateBoardColumnAsync(It.IsAny<UpdateBoardColumnModel>()))
                                         .Returns(Task.CompletedTask);
            var updateColumn = new UpdateBoardColumnModel { Id = 1, Name = "In Progress", Position = 1, WipLimit = 3 };

            var response = await _client.PutAsJsonAsync("/api/boardcolumns", updateColumn);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await ResponseHelper.GetApiResultAsync<string>(response);
            CheckResponse.Succeeded(body);
            body.Result.Should().Be("Board column updated successfully");
        }
    }
}
