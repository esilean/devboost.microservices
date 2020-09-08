using DroneDelivery.Pagamento.Application.CommandHandlers.Pagamentos;
using DroneDelivery.Pagamento.Application.Commands.Pagamentos;
using DroneDelivery.Pagamento.Application.Interfaces;
using DroneDelivery.Pagamento.Data.Repositorios.Interfaces;
using DroneDelivery.Pagamento.Domain.Models;
using DroneDelivery.Shared.Domain.Core.Bus;
using DroneDelivery.Shared.Domain.Core.Enums;
using DroneDelivery.Shared.Domain.Core.Events;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DroneDelivery.Pagamento.Application.Tests.Pagamentos
{
    public class CriarPagamentoHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly CriarPagamentoHandler _handler;

        public CriarPagamentoHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<CriarPagamentoHandler>();
        }

        [Fact(DisplayName = "Não deve criar um pagamento com command invalido")]
        public async Task Pagamento_AoCriarumPagamentoComComandoInvalido_RetornarNotificacoesComFalha()
        {
            // Arrange
            var command = new CriarPagamentoCommand("", DateTime.Now, 0);

            // Act
            var responseResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(responseResult.HasFails);
        }

        [Fact(DisplayName = "Deve criar um pagamento com command valido")]
        public async Task Pagamento_AoCriarumPagamentoComComandoValido_RetornarSucesso()
        {


            // Arrange
            var pedidoId = Guid.NewGuid();
            var command = new CriarPagamentoCommand("4242424242424242", DateTime.Now.AddDays(1), 123);
            command.MarcarPedidoId(pedidoId);

            var pedido = Pedido.Criar(pedidoId, 999);
            var pagamento = PedidoPagamento.Criar(pedido, "4242424242424242", DateTime.Now.AddDays(1), 123);
            pedido.AdicionarPagamento(pagamento);
            pedido.AtualizarStatus(PedidoStatus.AguardandoPagamento);

            //Obter pedido
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.Pedidos.ObterPorIdAsync(pedido.Id))
                    .Returns(Task.FromResult(pedido));

            //Obter Gateway
            _mocker.GetMock<IGatewayPagamento>()
                    .Setup(p => p.EnviarParaPagamento(It.IsAny<Pedido>()))
                    .Returns(Task.FromResult("token"));

            //Obter EventBus
            _mocker.GetMock<IEventBus>()
                    .Setup(p => p.Publish(It.IsAny<Event>()))
                    .Returns(Task.CompletedTask);

            //adicionar pagamento pedido
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.Pedidos.AdicionarPagamentoAsync(pagamento))
                    .Returns(Task.CompletedTask);

            //Salvar operação
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.SaveAsync())
                    .Returns(Task.CompletedTask);



            // Act
            var responseResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(responseResult.HasFails);
            _mocker.GetMock<IUnitOfWork>().Verify(o => o.SaveAsync(), Times.Once);
        }
    }
}
