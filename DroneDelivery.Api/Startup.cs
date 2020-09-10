using AutoMapper;
using DroneDelivery.Api.Configs;
using DroneDelivery.Api.Documentation;
using DroneDelivery.Api.Middeware;
using DroneDelivery.Application.CommandHandlers.Usuarios;
using DroneDelivery.Application.Configs;
using DroneDelivery.Application.Interfaces;
using DroneDelivery.Data.Data;
using DroneDelivery.Domain.Models;
using DroneDelivery.IOC;
using DroneDelivery.Shared.Infra.Clients;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DroneDelivery.Api
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
            services.AddDbContext<DroneDbContext>(opts =>
            {
                opts.UseInMemoryDatabase("DroneInMemory");
            });

            ConfigureServices(services);
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddDbContext<DroneDbContext>(opts =>
            {
                opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            ConfigureServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddDbContext<DroneDbContext>(opts =>
            {
                opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            ConfigureServices(services);
        }


        public void ConfigureServices(IServiceCollection services)
        {


            var key = Configuration.GetSection("JwtSettings:SigningKey").Value;
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = Token.TokenParametersConfig(key);
            });

            services.AddMediatR(typeof(Startup).Assembly);
            services.AddAutoMapper(typeof(CriarUsuarioHandler).Assembly);
            services.AddControllers().AddNewtonsoftJson();

            services.Configure<DronePontoInicialConfig>(Configuration.GetSection("BaseDrone"));
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

            //Adicionar acesso aos endpoint do microservico de pagamento
            HttpPagamentoClient.Registrar(services, Configuration["UrlBasePagamento"]);

            Swagger.Configurar(services);
            DependencyContainer.RegisterServices(services);
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DroneDbContext context, IPasswordHasher<Usuario> passwordHasher, IGeradorToken geradorToken)
        {

            app.UseMiddleware(typeof(ErrorHandlerMiddleware));
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }


            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Drone Delivery Entrega V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            Seed.Seed.SeedData(context, passwordHasher, geradorToken);
        }


    }
}
