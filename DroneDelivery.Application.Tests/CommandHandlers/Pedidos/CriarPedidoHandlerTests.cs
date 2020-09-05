using DroneDelivery.Application.CommandHandlers.Pedidos;
using DroneDelivery.Application.Commands.Pedidos;
using DroneDelivery.Application.Configs;
using DroneDelivery.Application.Interfaces;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Interfaces;
using DroneDelivery.Domain.Models;
using DroneDelivery.Utility.Messages;
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
    public class CriarPedidoHandlerTests
    {

        private readonly AutoMocker _mocker;
        private readonly CriarPedidoHandler _handler;

        public CriarPedidoHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<CriarPedidoHandler>();
        }

        [Fact(DisplayName = "Não deve criar um pedido com command invalido")]
        public async Task Pedido_AoCriarumPedidoComComandoInvalido_RetornarNotificacoesComFalha()
        {
            // Arrange
            var command = new CriarPedidoCommand(0);

            // Act
            var responseResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(responseResult.HasFails);
        }


        [Fact(DisplayName = "Não deve criar um pedido com cliente nao autenticado")]
        public async Task Pedido_AoCriarumPedidoComClienteNaoAutenticado_RetornarNotificacoesComFalha()
        {
            // Arrange
            var command = new CriarPedidoCommand(1000);

            var usuarioId = Guid.NewGuid();

            //Usuario autenticado nao retorna nada
            _mocker.GetMock<IUsuarioAutenticado>()
                    .Setup(p => p.GetCurrentId());

            //Obter usuario
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.Usuarios.ObterPorIdAsync(usuarioId));

            // Act
            var responseResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(responseResult.HasFails);
            Assert.True(responseResult.Fails.Count() == 1);
            Assert.NotNull(responseResult.Fails.Select(x => x.Message == PedidoMessage.Erro_ClienteNaoEncontrado));
        }

        [Fact(DisplayName = "Não deve criar um pedido caso nao tenha drones cadastrados")]
        public async Task Pedido_AoCriarumPedidoSemDroneCadastrado_RetornarNotificacoesComFalha()
        {
            // Arrange
            var command = new CriarPedidoCommand(1000);

            var usuarioId = Guid.NewGuid();

            //Usuario autenticado nao retorna nada
            _mocker.GetMock<IUsuarioAutenticado>()
                    .Setup(p => p.GetCurrentId())
                    .Returns(usuarioId);

            //Obter usuario
            var usuario = Usuario.Criar("test", "test@test.com", 0, 0, UsuarioRole.Cliente);
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.Usuarios.ObterPorIdAsync(usuarioId))
                    .Returns(Task.FromResult(usuario));

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
            Assert.NotNull(responseResult.Fails.Select(x => x.Message == PedidoMessage.Erro_DroneNaoCadastrado));
        }


        [Fact(DisplayName = "Deve criar um pedido e relacionar a um drone disponivel")]
        public async Task Pedido_CriarPedidoERelacionarAUmDrone_Retornar200OK()
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

            var command = new CriarPedidoCommand(1000);
            var usuarioId = Guid.NewGuid();

            //Usuario autenticado
            _mocker.GetMock<IUsuarioAutenticado>()
                    .Setup(p => p.GetCurrentId())
                    .Returns(usuarioId);

            //Calcular tempo entrega
            _mocker.GetMock<ICalcularTempoEntrega>()
                    .Setup(p => p.ObterTempoEntregaEmMinutosIda(latitudeOrigem, longitudeOrigem, latitudeUsuario, longitudeUsuario, velocidadeDrone));

            //Obter ponto inicial do drone
            _mocker.GetMock<IOptions<DronePontoInicialConfig>>()
                    .Setup(p => p.Value)
                    .Returns(new DronePontoInicialConfig { Latitude = latitudeOrigem, Longitude = longitudeOrigem });

            //Obter usuario
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.Usuarios.ObterPorIdAsync(usuarioId))
                    .Returns(Task.FromResult(Usuario.Criar("test", "test@test.com", latitudeUsuario, longitudeUsuario, UsuarioRole.Cliente)));

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

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.HasFails);
            _mocker.GetMock<IUsuarioAutenticado>().Verify(o => o.GetCurrentId(), Times.Once);
            _mocker.GetMock<IUnitOfWork>().Verify(o => o.Pedidos.AdicionarAsync(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IUnitOfWork>().Verify(o => o.SaveAsync(), Times.Once);
        }


        [Fact(DisplayName = "Deve criar um pedido e não relacionar a um drone disponivel por falta de capacidade de peso")]
        public async Task Pedido_CriarPedidoENaoRelacionarAUmDronePorNaoTerCapacidade_Retornar200OKEPedidoFicaEmAberto()
        {
            // Arrange
            var latitudeOrigem = -23.5880684;
            var longitudeOrigem = -46.6564195;

            var latitudeUsuario = -23.5950753;
            var longitudeUsuario = -46.645421;

            var velocidadeDrone = 3.333;

            IEnumerable<Drone> drones = new List<Drone> {
                Drone.Criar(500, velocidadeDrone, 35, 100, DroneStatus.Livre)
            };

            
            var command = new CriarPedidoCommand(1000);
            var usuarioId = Guid.NewGuid();

            //Usuario autenticado
            _mocker.GetMock<IUsuarioAutenticado>()
                    .Setup(p => p.GetCurrentId())
                    .Returns(usuarioId);

            //Calcular tempo entrega
            _mocker.GetMock<ICalcularTempoEntrega>()
                    .Setup(p => p.ObterTempoEntregaEmMinutosIda(latitudeOrigem, longitudeOrigem, latitudeUsuario, longitudeUsuario, velocidadeDrone));

            //Obter ponto inicial do drone
            _mocker.GetMock<IOptions<DronePontoInicialConfig>>()
                    .Setup(p => p.Value)
                    .Returns(new DronePontoInicialConfig { Latitude = latitudeOrigem, Longitude = longitudeOrigem });

            //Obter usuario
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.Usuarios.ObterPorIdAsync(usuarioId))
                    .Returns(Task.FromResult(Usuario.Criar("test", "test@test.com", latitudeUsuario, longitudeUsuario, UsuarioRole.Cliente)));

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

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.HasFails);
            _mocker.GetMock<IUsuarioAutenticado>().Verify(o => o.GetCurrentId(), Times.Once);
            _mocker.GetMock<IUnitOfWork>().Verify(o => o.Pedidos.AdicionarAsync(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IUnitOfWork>().Verify(o => o.SaveAsync(), Times.Once);
        }


    }
}
