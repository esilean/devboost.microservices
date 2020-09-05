using DroneDelivery.Api.Tests.Config;
using DroneDelivery.Application.Commands.Drones;
using DroneDelivery.Application.Commands.Pedidos;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DroneDelivery.Api.Tests.Controllers
{
    [Collection(nameof(IntegrationTestsFixtureCollection))]
    public class PedidosControllerTests
    {
        private readonly IntegrationTestsFixture<Startup> _testsFixture;

        public PedidosControllerTests(IntegrationTestsFixture<Startup> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Deve criar um pedido no banco de dados")]
        public async Task Pedido_CriarPedido_Retornar200OKSucesso()
        {
            // Arrange
            var drone = new CriarDroneCommand(12000, 3.333, 35, 100);

            await _testsFixture.RealizarLoginApi();
            _testsFixture.Client.AddToken(_testsFixture.Token);
            await _testsFixture.Client.PostAsJsonAsync("/api/drones", drone);

            // Arrange
            var pedido = new CriarPedidoCommand(10000, 100);

            await _testsFixture.RealizarLoginApi();
            _testsFixture.Client.AddToken(_testsFixture.Token);

            // Act
            var postResponse = await _testsFixture.Client.PostAsJsonAsync("/api/pedidos", pedido);

            // Assert
            postResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Deve retornar pedidos do cliente")]
        public async Task Pedido_RetornarPedidos_200OKSucesso()
        {
            // Arrange
            await _testsFixture.RealizarLoginApi();
            _testsFixture.Client.AddToken(_testsFixture.Token);

            // Act
            var response = await _testsFixture.Client.GetAsync("/api/pedidos");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
