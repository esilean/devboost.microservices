using DroneDelivery.Pagamento.Api.Tests.Config;
using DroneDelivery.Pagamento.Application.Commands.Pagamentos;
using DroneDelivery.Pagamento.Application.Commands.Pedidos;
using DroneDelivery.Pagamento.Domain.Enums;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace DroneDelivery.Pagamento.Api.Tests.BDD
{
    [Binding]
    public class WebHookReceberPagamentoSteps
    {

        private readonly ScenarioContext _context;
        private readonly IntegrationTestsFixture<Startup> _testsFixture;

        public WebHookReceberPagamentoSteps(
                ScenarioContext context,
                IntegrationTestsFixture<Startup> testsFixture)
        {
            _context = context;
            _testsFixture = testsFixture;
        }


        [Given(@"Que eu tenha um pedido que foi pago e esteja aguardando aprovação")]
        public async Task DadoQueEuTenhaUmPedidoQueFoiPagoEEstejaAguardandoAprovacao()
        {
            var command = new CriarPedidoCommand(Guid.NewGuid(), 999);
            _context.Add("pedido", command);
            await _testsFixture.Client.PostAsJsonAsync("/api/pedidos", command);

            var commandPagamento = new CriarPagamentoCommand("4242424242424242", DateTime.Now.AddDays(1), 123);
            await _testsFixture.Client.PostAsJsonAsync($"/api/pagamentos/{command.Id}", commandPagamento);
        }

        [Given(@"o webhook me retornar a situação deste pedido com status StatusPagamento: '(.*)'")]
        public void DadoOWebhookMeRetornarASituacaoDestePedidoComStatusStatusPagamento(string p0)
        {
            _context.Add("statusPagamento", p0);
        }

        [When(@"eu receber esta atualização")]
        public async Task QuandoEuReceberEstaAtualizacao()
        {
            var pedido = _context.Get<CriarPedidoCommand>("pedido");

            var status = _context.Get<string>("statusPagamento");
            var pagamentoStatus = PagamentoStatus.Aguardando;
            if (status.Contains("Aprovado", StringComparison.InvariantCultureIgnoreCase))
                pagamentoStatus = PagamentoStatus.Aprovado;
            else if (status.Contains("Reprovado", StringComparison.InvariantCultureIgnoreCase))
                pagamentoStatus = PagamentoStatus.Reprovado;

            var command = new WebhookPagamentoCommand(pedido.Id, pagamentoStatus);
            var postResponse = await _testsFixture.Client.PostAsJsonAsync($"/api/pagamentos/webhook", command);

            _context.Set(postResponse);
        }

        [Then(@"o pedido deverá ser aprovado ou reprovado")]
        public void EntaoOPedidoDeveraSerAprovadoOuReprovado()
        {
            var response = _context.Get<HttpResponseMessage>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
