using DroneDelivery.Api.Tests.Config;
using DroneDelivery.Api.Tests.Dtos.Drones;
using DroneDelivery.Api.Tests.Dtos.Pedidos;
using DroneDelivery.Application.Commands.Drones;
using DroneDelivery.Application.Commands.Pedidos;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Shared.Domain.Core.Enums;
using Newtonsoft.Json;
using System;
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

        [Given(@"que existam drones")]
        public async Task DadoQueExistamDrones()
        {
            //logar cliente
            await _testsFixture.RealizarLoginApi();
            _testsFixture.Client.AddToken(_testsFixture.Token);
            //_context.Set("token", _testsFixture.Token);

            //criar um drone
            var drone = new CriarDroneCommand(12000, 3.333, 35, 100);
            await _testsFixture.Client.PostAsJsonAsync("/api/drones", drone);

        }

        [Given(@"que este drone possua um pedido pago")]
        public async Task DadoQueEsteDronePossuaUmPedidoPago()
        {
            //criar pedido
            var command = new CriarPedidoCommand(1000, 999);
            await _testsFixture.Client.PostAsJsonAsync("/api/pedidos", command);

            var response = await _testsFixture.Client.GetAsync("/api/pedidos");
            var data = await response.Content.ReadAsStringAsync();
            var pedidos = JsonConvert.DeserializeObject<PedidosTestDto>(data);

            var pedido = pedidos.Pedidos.FirstOrDefault();

            if (pedido == null)
                throw new Exception("Para rodar os testes de integração é necessário que os dois microserviços sejam executados juntos");

            var respPagamentoDto = new CriarRepostaPagamentoDtoTests
            {
                Id = pedido.Id,
                Status = PedidoStatus.Pago
            };
            await _testsFixture.Client.PostAsJsonAsync("/api/pedidos/atualizarstatus", respPagamentoDto);
        }

        [When(@"eu solicitar o status dos drones")]
        public async Task QuandoEuSolicitarOStatusDosDrones()
        {
            var getResponse = await _testsFixture.Client.GetAsync("api/drones/situacao");
            _context.Set(getResponse);
        }

        [Then(@"a resposta devera ser um statuscode OK e retornar os drones")]
        public async Task EntaoARespostaDeveraSerUmStatuscodeOKERetornarOsDrones()
        {
            var response = _context.Get<HttpResponseMessage>();
            var data = await response.Content.ReadAsStringAsync();
            var drones = JsonConvert.DeserializeObject<DroneSituacaoTestDto>(data);

            Assert.True(drones.Drones.Select(x => x.Pedidos).Any());
        }

    }
}
