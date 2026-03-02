using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using SimpleKanbanBoards.Business.Models.Role;
using SimpleKanbanBoards.Business.Models.User;
using SimpleKanbanBoards.Business.Utils;
using Xunit;

namespace SimpleKanbanBoards.Tests.Unit.Utils
{
    public class JwtUtilTests
    {
        private readonly IConfiguration _configuration;

        public JwtUtilTests()
        {
            var settings = new Dictionary<string, string>
        {
            { "JWT:SecretKey", "super-secret-key-used-for-testing-1234567890" },
            { "JWT:Issuer", "TestIssuer" },
            { "JWT:Audience", "TestAudience" },
            { "JWT:ExpirationMinutes", "30" }
        };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(settings!)
                .Build();
        }

        [Fact]
        public void GenerateToken_ShouldReturnValidJwtString()
        {
            var user = new UserModel
            {
                Id = 1,
                UserName = "Akira",
                Roles = new List<RoleModel> { new RoleModel { Id = 1, Name = "Developer" } }
            };

            var token = JwtUtil.GenerateToken(user, _configuration);

            token.Should().NotBeNullOrEmpty();
            token.Split('.').Should().HaveCount(3);
        }

        [Fact]
        public void GenerateToken_ShouldContainUserIdAndUsernameInClaims()
        {
            var user = new UserModel { Id = 912, UserName = "Davo", Roles = new List<RoleModel>() };

            var token = JwtUtil.GenerateToken(user, _configuration);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            jwtToken.Claims.Should().Contain(c => c.Type == "nameid" && c.Value == "912");
            jwtToken.Claims.Should().Contain(c => c.Type == "unique_name" && c.Value == "Davo");
        }

        [Fact]
        public void GenerateToken_ShouldContainAllRolesInClaims()
        {
            var user = new UserModel
            {
                Id = 1,
                UserName = "manager",
                Roles = new List<RoleModel>
                {
                    new RoleModel { Id = 1, Name = "Project Manager" },
                    new RoleModel { Id = 2, Name = "Developer" }
                }
            };

            var token = JwtUtil.GenerateToken(user, _configuration);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var roles = jwtToken.Claims
                .Where(c => c.Type == "role")
                .Select(c => c.Value)
                .ToList();

            roles.Should().Contain("Project Manager");
            roles.Should().Contain("Developer");
        }

        [Fact]
        public void GenerateToken_ShouldExpireAfterConfiguredMinutes()
        {
            var user = new UserModel { Id = 1, UserName = "test", Roles = new List<RoleModel>() };

            var before = DateTime.UtcNow;
            var token = JwtUtil.GenerateToken(user, _configuration);
            var after = DateTime.UtcNow;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            jwtToken.ValidTo.Should().BeCloseTo(before.AddMinutes(30), precision: TimeSpan.FromSeconds(10));
        }
    }
}
