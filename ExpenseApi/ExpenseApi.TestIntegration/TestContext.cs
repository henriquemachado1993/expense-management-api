using ExpenseApi.Infra.Dependencies;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;

namespace ExpenseApi.TestIntegration
{
    public class TestContext
    {
        public HttpClient Client { get; set; }

        public TestContext()
        {
            SetupClient();
        }

        public void SetupClient()
        {
            var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    DependenciesInjector.Register(services);
                });
            });
            Client = application.CreateClient();
        }
    }
}
