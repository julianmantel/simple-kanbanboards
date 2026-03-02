using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SimpleKanbanBoards.Business.Models;
using SimpleKanbanBoards.Business.Models.User;
using SimpleKanbanBoards.Tests.Integration.Common;
using SimpleKanbanBoards.Tests.Integration.Helpers.ApiHelpers;
using Xunit;

namespace SimpleKanbanBoards.Tests.Integration.Controllers
{
    public class UsersControllerTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly TestWebApplicationFactory _factory;

        public UsersControllerTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GET_User_WhenUserExists_ShouldReturn200()
        {
            var user = new UserModel { Id = 1, UserName = "Vergil" };
            _factory.UserServiceMock.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(user);

            var response = await _client.GetAsync("/api/users/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await ResponseHelper.GetApiResultAsync<UserModel>(response);
            CheckResponse.Succeeded(body);
            body.Result.UserName.Should().Be("Vergil");
        }

        [Fact]
        public async Task GET_User_WhenUserDoesNotExist_ShouldReturn404()
        {
            _factory.UserServiceMock.Setup(s => s.GetUserByIdAsync(999)).ReturnsAsync((UserModel?)null);

            var response = await _client.GetAsync("/api/users/999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var body = await ResponseHelper.GetApiResultAsync<string>(response);
            body!.Errors.Should().Contain("User not found");
        }

        [Fact]
        public async Task POST_Register_WithValidPayload_ShouldReturn200()
        {
            _factory.UserServiceMock.Setup(s => s.CreateUserAsync(It.IsAny<CreateUserModel>()))
                                    .Returns(Task.CompletedTask);

            var payload = new CreateUserModel
            {
                UserName = "Artorias",
                Password = "Password1",
                Email = "oolacile@email.com",
                Roles = new List<int> { 1 }
            };

            var response = await _client.PostAsJsonAsync("/api/users", payload);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await ResponseHelper.GetApiResultAsync<string>(response);
            CheckResponse.Succeeded(body);
            body.Result.Should().Contain("Artorias");
        }

        [Fact]
        public async Task POST_Login_WithValidCredentials_ShouldReturn200AndSetCookie()
        {
            _factory.UserServiceMock.Setup(s => s.LoginAsync(It.IsAny<LoginRequestModel>()))
                                    .ReturnsAsync("fake.jwt.token");

            var payload = new LoginRequestModel { UserName = "GLaDOS", Password = "PotatOS" };

            var response = await _client.PostAsJsonAsync("/api/users/login", payload);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Headers.Should().ContainKey("Set-Cookie");
        }

        [Fact]
        public async Task POST_ResetPassword_ShouldReturn200()
        {
            _factory.UserServiceMock.Setup(s => s.ResetPasswordAsync(It.IsAny<ResetPassRequestModel>()))
                                    .Returns(Task.CompletedTask);

            var payload = new ResetPassRequestModel { Email = "user@email.com" };

            var response = await _client.PostAsJsonAsync("/api/users/reset-password", payload);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task PUT_ChangePassword_ShouldReturn200()
        {
            _factory.UserServiceMock.Setup(s => s.ChangePasswordAsync(It.IsAny<ChangePasswordModel>()))
                                    .Returns(Task.CompletedTask);

            var payload = new ChangePasswordModel { Token = "sometoken", NewPassword = "NewPass1" };

            var response = await _client.PutAsJsonAsync("/api/users/change-password", payload);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DELETE_User_ShouldReturn200()
        {
            _factory.UserServiceMock.Setup(s => s.DeleteUserAsync(1)).Returns(Task.CompletedTask);

            var response = await _client.DeleteAsync("/api/users/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await ResponseHelper.GetApiResultAsync<string>(response);
            CheckResponse.Succeeded(body);
        }
    }
}