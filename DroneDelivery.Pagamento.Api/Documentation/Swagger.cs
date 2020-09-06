using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace DroneDelivery.Pagamento.Api.Documentation
{
    public static class Swagger
    {
        public static void Configurar(IServiceCollection services)
        {
            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Drone Delivery Pagamento API",
                    Description = "Pagamento de Pedidos",
                    Contact = new OpenApiContact
                    {
                        Name = "Grupo 5",
                    }
                });


                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opts.IncludeXmlComments(xmlPath);
            });
        }

    }
}
