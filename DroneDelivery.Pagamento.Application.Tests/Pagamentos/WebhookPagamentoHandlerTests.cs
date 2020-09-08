using DroneDelivery.Pagamento.Application.CommandHandlers.Pagamentos;
using DroneDelivery.Pagamento.Application.Commands.Pagamentos;
using DroneDelivery.Pagamento.Data.Repositorios.Interfaces;
using DroneDelivery.Pagamento.Domain.Enums;
using DroneDelivery.Pagamento.Domain.Models;
using DroneDelivery.Shared.Domain.Core.Bus;
using DroneDelivery.Shared.Domain.Core.Enums;
using DroneDelivery.Shared.Domain.Core.Events;
using Moq;
using Moq.AutoMock;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DroneDelivery.Pagamento.Application.Tests.Pagamentos
{
    public class WebhookPagamentoHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly WebhookPagamentoHandler _handler;

        public WebhookPagamentoHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<WebhookPagamentoHandler>();
        }

        [Fact(DisplayName = "Não deve criar um webhook com command invalido")]
        public async Task Webhook_AoCriarumWEbhookComComandoInvalido_RetornarNotificacoesComFalha()
        {
            // Arrange
            var command = new WebhookPagamentoCommand(Guid.Empty, PagamentoStatus.Aprovado);

            // Act
            var responseResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(responseResult.HasFails);
        }

        [Fact(DisplayName = "Deve aprovar pagamento de um pedido recebido")]
        public async Task Webhook_AoCriarumPedidoComClienteNaoAutenticado_RetornarSucesso()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var command = new WebhookPagamentoCommand(pedidoId, PagamentoStatus.Aprovado);

            var pedido = Pedido.Criar(pedidoId, 999);
            var pagamento = PedidoPagamento.Criar(pedido, "4242424242424242", DateTime.Now.AddDays(1), 123);
            pedido.AdicionarPagamento(pagamento);
            pedido.AtualizarStatus(PedidoStatus.ProcessandoPagamento);

            //Obter pedido
            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.Pedidos.ObterPorIdAsync(pedido.Id))
                    .Returns(Task.FromResult(pedido));

            //Obter EventBus
            _mocker.GetMock<IEventBus>()
                    .Setup(p => p.Publish(It.IsAny<Event>()))
                    .Returns(Task.CompletedTask);

            // Act
            var responseResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(responseResult.HasFails);
            _mocker.GetMock<IUnitOfWork>().Verify(o => o.SaveAsync(), Times.Once);
        }
    }
}
