using DroneDelivery.Api.Tests.Config;
using DroneDelivery.Api.Tests.Dtos.Drones;
using DroneDelivery.Application.Commands.Drones;
using DroneDelivery.Application.Commands.Pedidos;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace DroneDelivery.Api.Tests.BDD
{
    [Binding]
    public class ObterSituacaoDroneSteps
    {

        private readonly ScenarioContext _context;
        private readonly IntegrationTestsFixture<Startup> _testsFixture;

        public ObterSituacaoDroneSteps(ScenarioContext context, IntegrationTestsFixture<Startup> testsFixture)
        {
            _testsFixture = testsFixture;
            _context = context;
        }


        [Given(@"que este drone possua um pedido")]
        public async Task DadoQueEsteDronePossuaUmPedido()
        {
            //logar cliente
            await _testsFixture.RealizarLoginApi();
            _testsFixture.Client.AddToken(_testsFixture.Token);

            //criar um drone
            var drone = new CriarDroneCommand(12000, 3.333, 35, 100);

            var postResponse = await _testsFixture.Client.PostAsJsonAsync("/api/drones", drone);
            _context.Set(postResponse);

            //criar pedido
            var pedido = new CriarPedidoCommand(10000);
            await _testsFixture.Client.PostAsJsonAsync("/api/pedidos", pedido);

        }

        [When(@"eu solicitar o status do drone")]
        public async Task QuandoEuSolicitarOStatusDoDrone()
        {
            var getResponse = await _testsFixture.Client.GetAsync("api/drones/situacao");
            _context.Set(getResponse);
        }

        [Then(@"devera retornar os pedidos do drone")]
        public async Task EntaoDeveraRetornarOsPedidosDoDrone()
        {
            var response = _context.Get<HttpResponseMessage>();
            var data = await response.Content.ReadAsStringAsync();
            var drones = JsonConvert.DeserializeObject<DroneSituacaoTestDto>(data);

            Assert.True(drones.Drones.Select(x => x.Pedidos).Any());
        }
    }
}
