using DroneDelivery.Pagamento.Application.Dtos;
using DroneDelivery.Pagamento.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar(CriarPedidoDto criarPedidoDto)
        {
            await _pedidoService.CriarPedido(criarPedidoDto);

            return Ok();
        }

    }
}
