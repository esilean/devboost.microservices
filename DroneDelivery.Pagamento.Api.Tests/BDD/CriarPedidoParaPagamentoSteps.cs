using DroneDelivery.Application.Dtos.Pedido;
using DroneDelivery.Pagamento.Api.Tests.Config;
using DroneDelivery.Shared.Domain.Core.Events.Pedidos;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace DroneDelivery.Pagamento.Api.Tests.BDD
{
    [Binding]
    public class CriarPedidoParaPagamentoSteps
    {
        private readonly ScenarioContext _context;
        private readonly IntegrationTestsFixture<Startup> _testsFixture;

        public CriarPedidoParaPagamentoSteps(ScenarioContext context, IntegrationTestsFixture<Startup> testsFixture)
        {
            _context = context;
            _testsFixture = testsFixture;
        }


        [Given(@"Que eu receba um Id do Pedido e Valor")]
        public void DadoQueEuRecebaUmIdDoPedidoEValor()
        {
            _context.Add("pedidoId", Guid.NewGuid());
            _context.Add("valor", 999.99);
        }

        [When(@"eu solicitar a criação do pedido")]
        public async Task QuandoEuSolicitarACriacaoDoPedido()
        {
            var pedido = new PedidoCriadoEvent(_context.Get<Guid>("pedidoId"), _context.Get<double>("valor"));


            var postResponse = await _testsFixture.Client.PostAsJsonAsync("/api/pedidos", pedido);

            _context.Set(postResponse);
        }

        [Then(@"o pedido deverá ser cadastrado a resposta devera ser um status code OK")]
        public void EntaoOPedidoDeveraSerCadastradoARespostaDeveraSerUmStatusCodeOK()
        {
            var response = _context.Get<HttpResponseMessage>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


    }
}
