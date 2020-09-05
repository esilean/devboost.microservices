using DroneDelivery.Api.Tests.Config;
using DroneDelivery.Application.Commands.Drones;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DroneDelivery.Api.Tests.Controllers
{
    [Collection(nameof(IntegrationTestsFixtureCollection))]
    public class DronesControllerTests
    {
        private readonly IntegrationTestsFixture<Startup> _testsFixture;

        public DronesControllerTests(IntegrationTestsFixture<Startup> testsFixture)
        {
            _testsFixture = testsFixture;
        }


        [Fact(DisplayName = "Deve criar um drone no banco de dados")]
        public async Task Drone_CriarDrone_Retornar200OKSucesso()
        {
            // Arrange
            var drone = new CriarDroneCommand(12000, 3.333, 35, 100);

            await _testsFixture.RealizarLoginApi();
            _testsFixture.Client.AddToken(_testsFixture.Token);

            // Act
            var postResponse = await _testsFixture.Client.PostAsJsonAsync("/api/drones", drone);

            // Assert
            postResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Não deve criar um drone no banco de dados")]
        public async Task Drone_NaoCriarDrone_Retornar400BadRequest()
        {
            // Arrange
            var drone = new CriarDroneCommand(0, 0, 35, 100);

            await _testsFixture.RealizarLoginApi();
            _testsFixture.Client.AddToken(_testsFixture.Token);

            // Act
            var postResponse = await _testsFixture.Client.PostAsJsonAsync("/api/drones", drone);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);
        }

        [Fact(DisplayName = "Deve retornar lista de drones")]
        public async Task Drone_RetornarListaDrones_Retornar200OK()
        {
            // Arrange
            await _testsFixture.RealizarLoginApi();
            _testsFixture.Client.AddToken(_testsFixture.Token);

            // Act
            var response = await _testsFixture.Client.GetAsync("/api/drones");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
