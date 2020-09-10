using DroneDelivery.Pagamento.Api.Documentation;
using DroneDelivery.Pagamento.Api.Middeware;
using DroneDelivery.Pagamento.Data.Data;
using DroneDelivery.Pagamento.IOC;
using DroneDelivery.Shared.Infra.Clients;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace DroneDelivery.Pagamento.Api
{

    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureTestingServices(IServiceCollection services)
        {
            services.AddDbContext<DronePgtoDbContext>(opts =>
            {
                opts.UseInMemoryDatabase("DronePgtoInMemory");
            });

            ConfigureServices(services);
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddDbContext<DronePgtoDbContext>(opts =>
            {
                opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            ConfigureServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddDbContext<DronePgtoDbContext>(opts =>
            {
                opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            ConfigureServices(services);
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            services.AddMediatR(typeof(Startup).Assembly);

            //Adicionar acesso aos endpoint do microservico de entregas
            HttpPedidoClient.Registrar(services, Configuration["UrlBasePedido"]);

            Swagger.Configurar(services);
            DependencyContainer.RegisterServices(services);
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseMiddleware(typeof(ErrorHandlerMiddleware));
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Drone Delivery Pagamento V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
