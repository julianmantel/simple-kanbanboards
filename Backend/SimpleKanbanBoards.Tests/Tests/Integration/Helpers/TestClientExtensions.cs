using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using SimpleKanbanBoards.Tests.Integration.Common;

namespace SimpleKanbanBoards.Tests.Integration.Helpers
{
    public static class TestClientExtensions
    {
        public static HttpClient CreateAuthenticatedClient(
        this TestWebApplicationFactory factory,
        string role = "Developer")
        {
            var client = factory.CreateClient();
            
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "test-token");
            
            client.DefaultRequestHeaders.Add(TestAuthHandler.RoleHeader, role);
            return client;
        }
    }
}
