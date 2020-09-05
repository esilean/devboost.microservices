using DroneDelivery.Pagamento.Domain.Models;
using DroneDelivery.Pagamento.Api.Tests.Config;
using System;
using TechTalk.SpecFlow;
using System.Net.Http;
using DroneDelivery.Pagamento.Application.Dtos;
using System.Threading.Tasks;
using System.Net;
using Xunit;

namespace DroneDelivery.Pagamento.Api.Tests.BDD
{
    [Binding]
    public class RealizarPagamentoSteps
    {

        private readonly ScenarioContext _context;
        private readonly IntegrationTestsFixture<Startup> _testsFixture;

        public RealizarPagamentoSteps(ScenarioContext context, IntegrationTestsFixture<Startup> testsFixture)
        {
            _context = context;
            _testsFixture = testsFixture;
        }

        [Given(@"Que eu tenha um pedido receba um NumeroCartao: '(.*)' Vencimento: '(.*)' CodigoSeguranca: '(.*)'")]
        public async Task DadoQueEuTenhaUmPedidoRecebaUmNumeroCartaoVencimentoCodigoSeguranca(string p0, string p1, int p2)
        {
            var pedidoDto = new CriarPedidoDto
            {
                Id = Guid.NewGuid(),
                Valor = 999
            };

            await _testsFixture.Client.PostAsJsonAsync("/api/pedidos", pedidoDto);


            _context.Add("pedido", pedidoDto);
            _context.Add("numeroCartao", p0);
            _context.Add("vencimento", Convert.ToDateTime(p1));
            _context.Add("codigoSeguranca", p2);
        }


        [When(@"eu solicitar a realização do pagamento")]
        public async Task QuandoEuSolicitarARealizacaoDoPagamento()
        {
            var pagamentoDto = new CriarPedidoPagamentoDto
            {
                NumeroCartao = _context.Get<string>("numeroCartao"),
                VencimentoCartao = _context.Get<DateTime>("vencimento"),
                CodigoSeguranca = _context.Get<int>("codigoSeguranca")
            };

            var pedido = _context.Get<CriarPedidoDto>("pedido");

            var postResponse = await _testsFixture.Client.PostAsJsonAsync($"/api/pagamentos/{pedido.Id}", pagamentoDto);

            _context.Set(postResponse);
        }

        [Then(@"o pedido deverá ser pago a resposta devera ser um status code (.*)OK")]
        public void EntaoOPedidoDeveraSerPagoARespostaDeveraSerUmStatusCodeOK(int p0)
        {
            var response = _context.Get<HttpResponseMessage>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
