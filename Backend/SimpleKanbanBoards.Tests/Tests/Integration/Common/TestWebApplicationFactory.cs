using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using SimpleKanbanBoards.Business.Service.IService;
using SimpleKanbanBoards.DataAccess.Models;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;
using SimpleKanbanBoards.Tests.Integration.Helpers;

namespace SimpleKanbanBoards.Tests.Integration.Common
{
    public class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<IUserRepository> UserRepositoryMock { get; } = new Mock<IUserRepository>();
        public Mock<IProjectRepository> ProjectRepositoryMock { get; } = new Mock<IProjectRepository>();

        public Mock<IBoardService> BoardServiceMock { get; } = new Mock<IBoardService>();
        public Mock<IProjectService> ProjectServiceMock { get; } = new Mock<IProjectService>();
        public Mock<IUserService> UserServiceMock { get; } = new Mock<IUserService>();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Replace real DbContext with in-memory for testing
                var dbDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<DbkanbanContext>));
                if (dbDescriptor != null) services.Remove(dbDescriptor);

                services.AddDbContext<DbkanbanContext>(options =>
                    options.UseInMemoryDatabase("TestDb_" + Guid.NewGuid()));

                // Remove existing authentication handlers and add a test one
                var jwtDescriptors = services
                .Where(d => d.ServiceType.Namespace != null &&
                            d.ServiceType.Namespace.Contains("Authentication"))
                .ToList();
                foreach (var d in jwtDescriptors) services.Remove(d);

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });

                // Replace services for testing with mocks
                RemoveAndReplace(services, BoardServiceMock.Object);
                RemoveAndReplace(services, ProjectServiceMock.Object);
                RemoveAndReplace(services, UserServiceMock.Object);

                RemoveAndReplace<IUserRepository>(services, UserRepositoryMock.Object);
                RemoveAndReplace<IProjectRepository>(services, ProjectRepositoryMock.Object);
            });

            builder.UseEnvironment("Development");
        }

        private static void RemoveAndReplace<TService>(IServiceCollection services, TService replacement) where TService : class
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(TService));
            if (descriptor != null) services.Remove(descriptor);
            services.AddScoped(_ => replacement);
        }
    }
}
