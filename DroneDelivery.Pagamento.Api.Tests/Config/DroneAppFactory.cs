using DroneDelivery.Pagamento.Api.Tests.MockHttpFacotry;
using DroneDelivery.Shared.Infra.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace DroneDelivery.Pagamento.Api.Tests.Config
{
    public class DroneAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting("https_port", "5001");
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.AddScoped<IPedidoHttpFactory, MockPedidoHttpFactory>();
            });
        }
    }
}
