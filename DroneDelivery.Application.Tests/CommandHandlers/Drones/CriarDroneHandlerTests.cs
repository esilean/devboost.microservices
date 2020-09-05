using DroneDelivery.Application.CommandHandlers.Drones;
using DroneDelivery.Application.Commands.Drones;
using Moq.AutoMock;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DroneDelivery.Application.Tests.CommandHandlers.Drones
{
    public class CriarDroneHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly CriarDroneHandler _handler;

        public CriarDroneHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<CriarDroneHandler>();
        }

        [Fact(DisplayName = "Não deve criar um drone com command invalido")]
        public async Task Drone_AoCriarumDroneComComandoInvalido_RetornarNotificacoesComFalha()
        {
            // Arrange
            var command = new CriarDroneCommand(0, 0, 0, 0);

            // Act
            var responseResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(responseResult.HasFails);
        }

        [Fact(DisplayName = "Não deve criar um drone com capacidade maior que a permitida")]
        public async Task Drone_AoCriarumDroneComCapacidadeMaiorquePermitida_RetornarNotificacoesComFalha()
        {
            // Arrange
            var command = new CriarDroneCommand(9999999, 3, 35, 100);

            // Act
            var responseResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(responseResult.HasFails);
        }
    }
}
