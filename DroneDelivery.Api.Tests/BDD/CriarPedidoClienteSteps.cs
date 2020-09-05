using DroneDelivery.Api.Tests.Config;
using DroneDelivery.Application.Commands.Drones;
using DroneDelivery.Application.Commands.Pedidos;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace DroneDelivery.Api.Tests.BDD
{
    [Binding]
    public class CriarPedidoClienteSteps
    {

        private readonly ScenarioContext _context;
        private readonly IntegrationTestsFixture<Startup> _testsFixture;

        public CriarPedidoClienteSteps(ScenarioContext context, IntegrationTestsFixture<Startup> testsFixture)
        {
            _testsFixture = testsFixture;
            _context = context;
        }

        [Given(@"que o cliente esteja logado")]
        public async Task DadoQueOClienteEstejaLogado()
        {
            await _testsFixture.RealizarLoginApi();
            _testsFixture.Client.AddToken(_testsFixture.Token);
        }

        [Given(@"que exista um drone")]
        public async Task DadoQueExistaUmDrone()
        {
            var drone = new CriarDroneCommand(12000, 3.333, 35, 100);

            await _testsFixture.Client.PostAsJsonAsync("/api/drones", drone);
        }


        [When(@"eu solicitar a criacao de um pedido")]
        public async Task QuandoEuSolicitarACriacaoDeUmPedido()
        {

            var pedido = new CriarPedidoCommand(10000, 100);

            await _testsFixture.RealizarLoginApi();
            _testsFixture.Client.AddToken(_testsFixture.Token);

            var postResponse = await _testsFixture.Client.PostAsJsonAsync("/api/pedidos", pedido);

            _context.Set(postResponse);
        }

        [Then(@"a resposta devera ser um status code 200OK")]
        public void EntaoARespostaDeveraSerUmStatusCodeOK()
        {
            var response = _context.Get<HttpResponseMessage>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
