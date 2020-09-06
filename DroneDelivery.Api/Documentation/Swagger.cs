using DroneDelivery.Api.Filter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace DroneDelivery.Api.Documentation
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
                    Title = "Drone Delivery Entrega API",
                    Description = "Entrega de Pedidos",
                    Contact = new OpenApiContact
                    {
                        Name = "Grupo 5",
                    }
                });
                opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Por favor insira um JWT com \"Bearer\" nesse campo",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                opts.OperationFilter<AuthOperationFilter>();


                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opts.IncludeXmlComments(xmlPath);
            });
        }

    }
}
