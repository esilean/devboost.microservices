using DroneDelivery.Pagamento.Api.Controllers;
using DroneDelivery.Pagamento.Application.Commands.Pagamentos;
using DroneDelivery.Shared.Domain.Core.Bus;
using DroneDelivery.Shared.Domain.Core.Commands;
using DroneDelivery.Shared.Domain.Core.Domain;
using Moq;
using Moq.AutoMock;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DroneDelivery.Pagamento.Api.Tests.Controllers
{
    public class PagamentosControllerTests
    {
        private readonly AutoMocker _mocker;
        private readonly PagamentosController _controller;

        public PagamentosControllerTests()
        {
            _mocker = new AutoMocker();
            _controller = _mocker.CreateInstance<PagamentosController>();
        }

        [Fact(DisplayName = "Deve enviar um command de pagamento valido")]
        public async Task Webhook_AoCriarumWEbhookComComandoInvalido_RetornarNotificacoesComFalha()
        {
            // Arrange
            var command = new CriarPagamentoCommand("4242424242424242", DateTime.Now.AddDays(1), 123);

            //Obter EventBus
            _mocker.GetMock<IEventBus>()
                    .Setup(p => p.SendCommand(It.IsAny<Command>()))
                    .Returns(Task.FromResult(new ResponseResult()));

            // Act
            var responseResult = await _controller.RealizarPagamento(Guid.NewGuid(), command);

            // Assert
            Assert.NotNull(responseResult);
        }
    }
}
