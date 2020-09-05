using DroneDelivery.Pagamento.Application.Dtos;
using DroneDelivery.Pagamento.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentosController : ControllerBase
    {
        private readonly IPedidoPagamentoService _pedidoPagamentoService;

        public PagamentosController(IPedidoPagamentoService pedidoPagamentoService)
        {
            _pedidoPagamentoService = pedidoPagamentoService;
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar(CriarPedidoPagamentoDto criarPedidoPagamentoDto)
        {
            await _pedidoPagamentoService.RealizarPagamento(criarPedidoPagamentoDto);

            return Ok();
        }

    }
}
