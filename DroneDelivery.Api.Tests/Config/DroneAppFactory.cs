using DroneDelivery.Api.Tests.MockHttpFactory;
using DroneDelivery.Shared.Infra.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace DroneDelivery.Api.Tests.Config
{
    public class DroneAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting("https_port", "5001");
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.AddScoped<IPagamentoHttpFactory, MockPagamentoHttpFactory>();
            });
        }
    }
}
