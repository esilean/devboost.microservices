using DroneDelivery.Application.CommandHandlers.Pedidos;
using DroneDelivery.Application.Commands.Pedidos;
using DroneDelivery.Application.Configs;
using DroneDelivery.Application.Interfaces;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Interfaces;
using DroneDelivery.Domain.Models;
using DroneDelivery.Shared.Domain.Core.Enums;
using DroneDelivery.Shared.Utility.Messages;
using Microsoft.Extensions.Options;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DroneDelivery.Application.Tests.CommandHandlers.Pedidos
{
    public class AtualizarPedidoStatusHandlerTests
    {

        private readonly AutoMocker _mocker;
        private readonly AtualizarPedidoStatusHandler _handler;

        public AtualizarPedidoStatusHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<AtualizarPedidoStatusHandler>();
        }

        [Fact(DisplayName = "Não deve criar atualizar um pedido com command invalido")]
        public async Task AtualizarPedido_AoCriarumAtualizarPedidoComComandoInvalido_RetornarNotificacoesComFalha()
        {
            // Arrange
            var command = new AtualizarPedidoStatusCommand(Guid.Empty, PedidoStatus.Pago);

            // Act
            var responseResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(responseResult.HasFails);
        }


        [Fact(DisplayName = "Não deve atualizar um pedido caso nao tenha drones cadastrados")]
        public async Task Pedido_AoAtualizarPedidoSemDroneCadastrado_RetornarNotificacoesComFalha()
        {
            // Arrange
            var command = new AtualizarPedidoStatusCommand(Guid.NewGuid(), PedidoStatus.Pago);

            var usuario = Usuario.Criar("test", "test@test.com", 0, 0, UsuarioRole.Cliente);

            //Usuario autenticado nao retorna nada
            _mocker.GetMock<IUsuarioAutenticado>()
                    .Setup(p => p.GetCurrentId())
                    .Returns(usuario.Id);

            //Obter usuario
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.Usuarios.ObterPorIdAsync(usuario.Id))
                    .Returns(Task.FromResult(usuario));

            //obter pedido
            var pedido = Pedido.Criar(1000, 1000, usuario);
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.Pedidos.ObterPorIdAsync(pedido.Id))
                    .Returns(Task.FromResult(pedido));



            //Criar lista de drones
            IEnumerable<Drone> drones = new List<Drone> { };
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.Drones.ObterTodosAsync())
                    .Returns(Task.FromResult(drones));

            // Act
            var responseResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(responseResult.HasFails);
            Assert.True(responseResult.Fails.Count() == 1);
            Assert.NotNull(responseResult.Fails.Select(x => x.Message == Erros.ErroDrone_NaoCadastrado));
        }


        [Fact(DisplayName = "Não deve atualizar um pedido caso status seja igual a pago")]
        public async Task Pedido_AoAtualizarPedidoPago_RetornarNotificacoesComFalha()
        {
            // Arrange
            var command = new AtualizarPedidoStatusCommand(Guid.NewGuid(), PedidoStatus.Pago);

            var usuario = Usuario.Criar("test", "test@test.com", 0, 0, UsuarioRole.Cliente);

            //obter pedido
            var pedido = Pedido.Criar(1000, 1000, usuario);
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.Pedidos.ObterPorIdAsync(pedido.Id))
                    .Returns(Task.FromResult(pedido));

            // Act
            var responseResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(responseResult.HasFails);
            Assert.True(responseResult.Fails.Count() == 1);
        }


        [Fact(DisplayName = "Deve atualizar pedido e relacionar a um drone disponivel")]
        public async Task Pedido_AtualizarPedidoERelacionarAUmDrone_Retornar200OK()
        {
            // Arrange
            var latitudeOrigem = -23.5880684;
            var longitudeOrigem = -46.6564195;

            var latitudeUsuario = -23.5950753;
            var longitudeUsuario = -46.645421;

            var velocidadeDrone = 3.333;

            IEnumerable<Drone> drones = new List<Drone> {
                Drone.Criar(12000, velocidadeDrone, 35, 100, DroneStatus.Livre)
            };


            var usuario = Usuario.Criar("test", "test@test.com", 0, 0, UsuarioRole.Cliente);

            //obter pedido
            var pedido = Pedido.Criar(1000, 1000, usuario);
            pedido.AtualizarStatusPedido(PedidoStatus.AguardandoPagamento);
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.Pedidos.ObterPorIdAsync(pedido.Id))
                    .Returns(Task.FromResult(pedido));

            //Calcular tempo entrega
            _mocker.GetMock<ICalcularTempoEntrega>()
                    .Setup(p => p.ObterTempoEntregaEmMinutosIda(latitudeOrigem, longitudeOrigem, latitudeUsuario, longitudeUsuario, velocidadeDrone));

            //Obter ponto inicial do drone
            _mocker.GetMock<IOptions<DronePontoInicialConfig>>()
                    .Setup(p => p.Value)
                    .Returns(new DronePontoInicialConfig { Latitude = latitudeOrigem, Longitude = longitudeOrigem });

            //Criar lista de drones
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.Drones.ObterTodosAsync())
                    .Returns(Task.FromResult(drones));

            //adicionar pedido
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.Pedidos.AdicionarAsync(It.IsAny<Pedido>()))
                    .Returns(Task.CompletedTask);

            //Salvar operação
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.SaveAsync())
                    .Returns(Task.CompletedTask);

            var command = new AtualizarPedidoStatusCommand(pedido.Id, PedidoStatus.AguardandoPagamento);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.HasFails);
            _mocker.GetMock<IUnitOfWork>().Verify(o => o.SaveAsync(), Times.Once);
        }


        [Fact(DisplayName = "Deve atualizar pedido e não relacionar a um drone disponivel")]
        public async Task Pedido_AtualizarPedidoENaoRelacionarAUmDrone_Retornar200OK()
        {
            // Arrange
            var latitudeOrigem = -23.5880684;
            var longitudeOrigem = -46.6564195;

            var latitudeUsuario = -23.5950753;
            var longitudeUsuario = -46.645421;

            var velocidadeDrone = 3.333;

            IEnumerable<Drone> drones = new List<Drone> {
                Drone.Criar(12000, velocidadeDrone, 35, 100, DroneStatus.EmManutencao)
            };


            var usuario = Usuario.Criar("test", "test@test.com", 0, 0, UsuarioRole.Cliente);

            //obter pedido
            var pedido = Pedido.Criar(1000, 1000, usuario);
            pedido.AtualizarStatusPedido(PedidoStatus.AguardandoPagamento);
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.Pedidos.ObterPorIdAsync(pedido.Id))
                    .Returns(Task.FromResult(pedido));

            //Calcular tempo entrega
            _mocker.GetMock<ICalcularTempoEntrega>()
                    .Setup(p => p.ObterTempoEntregaEmMinutosIda(latitudeOrigem, longitudeOrigem, latitudeUsuario, longitudeUsuario, velocidadeDrone));

            //Obter ponto inicial do drone
            _mocker.GetMock<IOptions<DronePontoInicialConfig>>()
                    .Setup(p => p.Value)
                    .Returns(new DronePontoInicialConfig { Latitude = latitudeOrigem, Longitude = longitudeOrigem });

            //Criar lista de drones
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.Drones.ObterTodosAsync())
                    .Returns(Task.FromResult(drones));

            //adicionar pedido
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.Pedidos.AdicionarAsync(It.IsAny<Pedido>()))
                    .Returns(Task.CompletedTask);

            //Salvar operação
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.SaveAsync())
                    .Returns(Task.CompletedTask);

            var command = new AtualizarPedidoStatusCommand(pedido.Id, PedidoStatus.AguardandoPagamento);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.HasFails);
            _mocker.GetMock<IUnitOfWork>().Verify(o => o.SaveAsync(), Times.Once);
        }






    }
}
