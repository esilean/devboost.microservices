using DroneDelivery.Shared.Utility.Events;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DroneDelivery.Shared.Infra.Clients
{
    public static class HttpPedidoClient
    {
        public static void Registrar(IServiceCollection services, string baseURL)
        {
            services.AddHttpClient(HttpClientName.PedidoEndPoint, opts =>
            {
                opts.BaseAddress = new Uri(baseURL);
            });
        }
    }
}
