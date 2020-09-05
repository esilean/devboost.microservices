using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DroneDelivery.Pagamento.Api.Tests.Config
{
    public class DroneAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting("https_port", "5001");
            //se for necessario criar outra startup class devemos marcar que queremos utilizar ela
            //builder.UseStartup<TStartup>();
            builder.UseEnvironment("Testing");
        }
    }
}
