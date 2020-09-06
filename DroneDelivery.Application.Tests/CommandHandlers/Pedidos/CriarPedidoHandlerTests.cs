using DroneDelivery.Application.CommandHandlers.Pedidos;
using DroneDelivery.Application.Commands.Pedidos;
using DroneDelivery.Application.Dtos.Pedido;
using DroneDelivery.Application.Interfaces;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Models;
using DroneDelivery.Shared.Domain.Core.Events;
using DroneDelivery.Shared.Domain.Core.Events.Pedidos;
using DroneDelivery.Shared.Infra.Interfaces;
using DroneDelivery.Shared.Utility.Messages;
using Moq;
using Moq.AutoMock;
using System;
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
            var command = new CriarPedidoCommand(0, 0);

            // Act
            var responseResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(responseResult.HasFails);
        }


        [Fact(DisplayName = "Não deve criar um pedido com cliente nao autenticado")]
        public async Task Pedido_AoCriarumPedidoComClienteNaoAutenticado_RetornarNotificacoesComFalha()
        {
            // Arrange
            var command = new CriarPedidoCommand(1000, 1000);

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
            Assert.NotNull(responseResult.Fails.Select(x => x.Message == Erros.ErroCliente_NaoEncontrado));
        }

        [Fact(DisplayName = "Deve criar um pedido")]
        public async Task Pedido_AoCriarUmPedido_RetornarSucesso()
        {
            // Arrange
            var command = new CriarPedidoCommand(1000, 1000);

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

            _mocker.GetMock<IPagamentoHttpFactory>()
                    .Setup(p => p.EnviarPedidoParaPagamento(It.IsAny<PedidoCriadoEvent>()));

            //adicionar pedido
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.Pedidos.AdicionarAsync(It.IsAny<Pedido>()))
                    .Returns(Task.CompletedTask);

            //Salvar operação
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.SaveAsync())
                    .Returns(Task.CompletedTask);

            // Act
            var responseResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(responseResult.HasFails);
        }


    }
}
