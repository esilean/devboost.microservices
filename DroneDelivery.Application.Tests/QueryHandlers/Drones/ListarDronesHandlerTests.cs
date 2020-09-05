using DroneDelivery.Application.Queries.Drones;
using DroneDelivery.Application.QueryHandlers.Drones;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Models;
using Moq;
using Moq.AutoMock;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DroneDelivery.Application.Tests.QueryHandlers.Drones
{
    public class ListarDronesHandlerTests
    {

        private readonly AutoMocker _mocker;
        private readonly ListarDronesHandler _handler;

        public ListarDronesHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<ListarDronesHandler>();
        }

        [Fact(DisplayName = "Deve retornar uma lista de drones")]
        public async Task Drone_SolicitarTodosDrones_RetornarListaDeDrones()
        {
            // Arrange
            var query = new DronesQuery();

            IEnumerable<Drone> drones = new List<Drone>
            {
                Drone.Criar(12000, 3, 35, 100, DroneStatus.Livre)
            };

            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.Drones.ObterTodosAsync())
                    .Returns(Task.FromResult(drones));


            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.HasFails);
            _mocker.GetMock<IUnitOfWork>().Verify(o => o.Drones.ObterTodosAsync(), Times.Once);
        }
    }
}
