using DroneDelivery.Domain.Core.Commands;
using DroneDelivery.Domain.Core.Domain;
using DroneDelivery.Domain.Core.Queries;
using MediatR;
using Moq;
using Moq.AutoMock;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DroneDelivery.Bus.Tests
{
    public class MediatorHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly MediatorHandler _handler;

        public MediatorHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<MediatorHandler>();
        }

        public class CommandTest : Command { }
        public class QueryTest : QueryFilter { }

        [Fact(DisplayName = "Deve enviar commands para seus handlers")]
        public async Task Mediator_AoEnviarComando_RetornarOK()
        {
            // Arrange
            var command = new CommandTest();
            _mocker.GetMock<IMediator>()
                .Setup(x => x.Send(It.IsAny<Command>(), CancellationToken.None))
                .Returns(Task.FromResult(new ResponseResult()));

            // Act
            var responseResult = await _handler.SendCommand(command);

            // Assert
            _mocker.GetMock<IMediator>().Verify(x => x.Send(It.IsAny<Command>(), CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Deve enviar queries para seus handlers")]
        public async Task Mediator_AoEnviarQuery_RetornarOK()
        {
            // Arrange
            var query = new QueryTest();
            _mocker.GetMock<IMediator>()
                .Setup(x => x.Send(It.IsAny<QueryFilter>(), CancellationToken.None))
                .Returns(Task.FromResult(new ResponseResult()));

            // Act
            var responseResult = await _handler.RequestQuery(query);

            // Assert
            _mocker.GetMock<IMediator>().Verify(x => x.Send(It.IsAny<QueryFilter>(), CancellationToken.None), Times.Once);
        }
    }

}
