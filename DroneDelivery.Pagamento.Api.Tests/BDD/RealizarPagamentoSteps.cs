using DroneDelivery.Pagamento.Api.Tests.Config;
using DroneDelivery.Pagamento.Application.Commands.Pagamentos;
using DroneDelivery.Pagamento.Application.Commands.Pedidos;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace DroneDelivery.Pagamento.Api.Tests.BDD
{
    [Binding]
    public class RealizarPagamentoSteps
    {

        private readonly ScenarioContext _context;
        private readonly IntegrationTestsFixture<Startup> _testsFixture;

        public RealizarPagamentoSteps(
                ScenarioContext context,
                IntegrationTestsFixture<Startup> testsFixture)
        {
            _context = context;
            _testsFixture = testsFixture;
        }


        [Given(@"Que eu tenha um pedido para pagamento receba um NumeroCartao: '(.*)' Vencimento: '(.*)' CodigoSeguranca: '(.*)'")]
        public async Task DadoQueEuTenhaUmPedidoParaPagamentoRecebaUmNumeroCartaoVencimentoCodigoSeguranca(string p0, string p1, int p2)
        {
            var command = new CriarPedidoCommand(Guid.NewGuid(), 999);

            await _testsFixture.Client.PostAsJsonAsync("/api/pedidos", command);

            _context.Add("pedido", command);
            _context.Add("numeroCartao", p0);
            _context.Add("vencimento", Convert.ToDateTime(p1));
            _context.Add("codigoSeguranca", p2);
        }

        [When(@"eu solicitar a realização do pagamento")]
        public async Task QuandoEuSolicitarARealizacaoDoPagamento()
        {
            var numeroCartao = _context.Get<string>("numeroCartao");
            var vencimentoCartao = _context.Get<DateTime>("vencimento");
            var codigoSeguranca = _context.Get<int>("codigoSeguranca");
            var command = new CriarPagamentoCommand(numeroCartao, vencimentoCartao, codigoSeguranca);

            var pedido = _context.Get<CriarPedidoCommand>("pedido");

            var postResponse = await _testsFixture.Client.PostAsJsonAsync($"/api/pagamentos/{pedido.Id}", command);

            _context.Set(postResponse);
        }

        [Then(@"o pedido deverá ser pago a resposta devera ser um status code OK")]
        public void EntaoOPedidoDeveraSerPagoARespostaDeveraSerUmStatusCodeOK()
        {
            var response = _context.Get<HttpResponseMessage>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
