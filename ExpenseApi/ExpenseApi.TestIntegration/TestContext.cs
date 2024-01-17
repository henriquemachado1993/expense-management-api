using ExpenseApi.Infra.Dependencies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
