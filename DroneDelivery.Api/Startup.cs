using AutoMapper;
using DroneDelivery.Api.Configs;
using DroneDelivery.Api.Filter;
using DroneDelivery.Application.CommandHandlers.Usuarios;
using DroneDelivery.Application.Configs;
using DroneDelivery.Application.Interfaces;
using DroneDelivery.Data.Data;
using DroneDelivery.Domain.Models;
using DroneDelivery.IOC;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace DroneDelivery.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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


        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Drone Delivery API",
                    Description = "Entrega de pedidos via drone",
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


            services.AddHttpClient("pagamentos", opts =>
            {
                opts.BaseAddress = new Uri(Configuration["UrlBasePagamento"]);
            });


            RegisterServices(services);
        }

        private void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DroneDbContext context, IPasswordHasher<Usuario> passwordHasher, IGeradorToken geradorToken)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Drone Delivery API V1");
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
