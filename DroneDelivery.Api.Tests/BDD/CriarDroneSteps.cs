using DroneDelivery.Api.Tests.Config;
using DroneDelivery.Application.Commands.Drones;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace DroneDelivery.Api.Tests.BDD
{
    [Binding]
    public class CriarDroneSteps
    {
        private readonly ScenarioContext _context;
        private readonly IntegrationTestsFixture<Startup> _testsFixture;

        public CriarDroneSteps(ScenarioContext context, IntegrationTestsFixture<Startup> testsFixture)
        {
            _testsFixture = testsFixture;
            _context = context;
        }

        [When(@"eu solicitar a criacao de drones com os detalhes  Capacidade: '(.*)' Velocidade: '(.*)' Autonomia: '(.*)' Carga: '(.*)'")]
        public async Task QuandoEuSolicitarACriacaoDeDronesComOsDetalhesCapacidadeVelocidadeAutonomiaCarga(int p0, double p1, int p2, int p3)
        {
            var drone = new CriarDroneCommand(p0, p1, p2, p3);

            _testsFixture.Client.AddToken(_testsFixture.Token);

            // Act
            var postResponse = await _testsFixture.Client.PostAsJsonAsync("/api/drones", drone);
            _context.Set(postResponse);
        }

        [Then(@"a resposta devera ser um status code 400BadRequest")]
        public void EntaoARespostaDeveraSerUmStatusCodeBadRequest()
        {
            var response = _context.Get<HttpResponseMessage>();
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

    }
}
