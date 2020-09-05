using DroneDelivery.Pagamento.Application.Interfaces;
using DroneDelivery.Pagamento.Application.Services;
using DroneDelivery.Pagamento.Data.Data;
using DroneDelivery.Pagamento.Data.Repositorios;
using DroneDelivery.Pagamento.Data.Repositorios.Interfaces;
using DroneDelivery.Shared.Infra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DroneDelivery.Pagamento.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<DronePgtoDbContext>(opts =>
            {
                opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddControllers();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPedidoService, PedidoService>();
            services.AddScoped<IPedidoPagamentoService, PedidoPagamentoService>();

            services.AddScoped<IEnviarRespostaPagamento, EnviarRespostaPagamento>();

            services.AddHttpClient("pedidos", opts =>
            {
                opts.BaseAddress = new Uri(Configuration["UrlBasePedido"]);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
