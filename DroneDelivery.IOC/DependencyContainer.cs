using DroneDelivery.Application.CommandHandlers.Drones;
using DroneDelivery.Application.CommandHandlers.Pedidos;
using DroneDelivery.Application.CommandHandlers.Usuarios;
using DroneDelivery.Application.Commands.Drones;
using DroneDelivery.Application.Commands.Pedidos;
using DroneDelivery.Application.Commands.Usuarios;
using DroneDelivery.Application.Interfaces;
using DroneDelivery.Application.Queries.Drones;
using DroneDelivery.Application.Queries.Pedidos;
using DroneDelivery.Application.Queries.Usuarios;
using DroneDelivery.Application.QueryHandlers.Drones;
using DroneDelivery.Application.QueryHandlers.Pedidos;
using DroneDelivery.Application.QueryHandlers.Usuarios;
using DroneDelivery.Application.Services;
using DroneDelivery.Application.Services.Interfaces;
using DroneDelivery.Data.Data;
using DroneDelivery.Data.Repositorios;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Interfaces;
using DroneDelivery.Domain.Models;
using DroneDelivery.Infra.AcessoUsuario;
using DroneDelivery.Infra.Jwt;
using DroneDelivery.Infra.TempoEntrega;
using DroneDelivery.Shared.Bus;
using DroneDelivery.Shared.Domain.Core.Bus;
using DroneDelivery.Shared.Domain.Core.Domain;
using DroneDelivery.Shared.Infra.HttpFactories;
using DroneDelivery.Shared.Infra.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DroneDelivery.IOC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {

            //mediator
            services.AddScoped<IEventBus, MediatorHandler>();
            //events
            services.AddScoped<IPagamentoHttpFactory, PagamentoHttpFactory>();
            services.AddScoped<IPedidoHttpFactory, PedidoHttpFactory>();


            //lib calcular tempo entrega 
            services.AddScoped<ICalcularTempoEntrega, CalcularTempoEntrega>();

            //serviços de autenticação
            services.AddScoped<JwtSettings>();
            services.AddScoped<IGeradorToken, GeradorToken>();
            services.AddScoped<IUsuarioAutenticado, UsuarioAutenticado>();
            services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();

            //commands
            services.AddScoped<IRequestHandler<LoginUsuarioCommand, ResponseResult>, LoginUsuarioHandler>();
            services.AddScoped<IRequestHandler<CriarUsuarioCommand, ResponseResult>, CriarUsuarioHandler>();

            services.AddScoped<IRequestHandler<CriarDroneCommand, ResponseResult>, CriarDroneHandler>();

            services.AddScoped<IRequestHandler<CriarPedidoCommand, ResponseResult>, CriarPedidoHandler>();
            services.AddScoped<IRequestHandler<AtualizarPedidoStatusCommand, ResponseResult>, AtualizarPedidoStatusHandler>();

            //queries
            services.AddScoped<IRequestHandler<UsuariosQuery, ResponseResult>, ListarUsuariosHandler>();
            services.AddScoped<IRequestHandler<RefreshTokenCommand, ResponseResult>, RefreshTokenHandler>();

            services.AddScoped<IRequestHandler<DronesQuery, ResponseResult>, ListarDronesHandler>();
            services.AddScoped<IRequestHandler<SituacaoDronesQuery, ResponseResult>, ListarSituacaoDronesHandler>();

            services.AddScoped<IRequestHandler<PedidosQuery, ResponseResult>, ListarPedidosHandler>();

            //services
            services.AddScoped<IAssociarPedidoDroneService, AssociarPedidoDroneService>();

            //repositórios
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<DroneDbContext>();
        }
    }
}
