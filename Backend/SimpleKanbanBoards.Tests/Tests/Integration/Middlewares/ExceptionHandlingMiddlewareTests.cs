using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SimpleKanbanBoards.Business.Exceptions;
using SimpleKanbanBoards.Business.Models.Board;
using SimpleKanbanBoards.Business.Models.User;
using SimpleKanbanBoards.Tests.Integration.Common;
using SimpleKanbanBoards.Tests.Integration.Helpers;
using Xunit;

namespace SimpleKanbanBoards.Tests.Integration.Middlewares
{
    public class ExceptionHandlingMiddlewareTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly TestWebApplicationFactory _factory;

        public ExceptionHandlingMiddlewareTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateAuthenticatedClient("Project Manager");
        }

        [Fact]
        public async Task WhenServiceThrowsNotFoundException_ShouldReturn404ProblemDetails()
        {
            _factory.BoardServiceMock.Setup(s => s.GetBoardByIdAsync(It.IsAny<int>()))
                                     .ThrowsAsync(new NotFoundException("Board not found."));

            var response = await _client.GetAsync("/api/boards/1");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            response.Content.Headers.ContentType!.MediaType.Should().Be("application/problem+json");

            var body = await response.Content.ReadFromJsonAsync<JsonElement>();
            body.GetProperty("title").GetString().Should().Be("Board not found.");
            body.GetProperty("status").GetInt32().Should().Be(404);
        }

        [Fact]
        public async Task WhenServiceThrowsConflictException_ShouldReturn409ProblemDetails()
        {
            _factory.BoardServiceMock.Setup(s => s.CreateBoardAsync(It.IsAny<BoardModel>()))
                                     .ThrowsAsync(new ConflictException("Already exists."));

            var payload = new BoardModel { Name = "Test", ProjectId = 1 };

            var response = await _client.PostAsJsonAsync("/api/boards", payload);

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            var body = await response.Content.ReadFromJsonAsync<JsonElement>();
            body.GetProperty("status").GetInt32().Should().Be(409);
        }

        [Fact]
        public async Task WhenServiceThrowsUnauthorizedException_ShouldReturn401ProblemDetails()
        {
            _factory.UserServiceMock.Setup(s => s.LoginAsync(It.IsAny<LoginRequestModel>()))
                                    .ThrowsAsync(new UnauthorizedException("Invalid credentials."));

            var payload = new LoginRequestModel { UserName = "user", Password = "wrong" };

            var response = await _client.PostAsJsonAsync("/api/users/login", payload);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task WhenUnhandledExceptionOccurs_ShouldReturn500ProblemDetails()
        {
            _factory.BoardServiceMock.Setup(s => s.GetBoardByIdAsync(It.IsAny<int>()))
                                     .ThrowsAsync(new InvalidOperationException("Something broke"));

            var response = await _client.GetAsync("/api/boards/1");

            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            var body = await response.Content.ReadFromJsonAsync<JsonElement>();
            body.GetProperty("title").GetString().Should().Be("Internal Server Error");
        }
    }
}