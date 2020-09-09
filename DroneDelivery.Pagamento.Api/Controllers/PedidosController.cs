using DroneDelivery.Pagamento.Application.Commands.Pedidos;
using DroneDelivery.Shared.Domain.Core.Bus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Api.Controllers
{

    public class PedidosController : BaseController
    {

        public PedidosController(IEventBus eventBus) : base(eventBus)
        {
        }


        /// <summary>
        /// Receber um pedido para pagamento
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/pedidos
        ///     {
        ///        "id": "2e4f41f4-dfd2-4464-8896-d39e22315cdd",
        ///        "valor": 999
        ///     }
        ///
        /// </remarks>        
        /// <param name="command"></param>  
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Criar(CriarPedidoCommand command)
        {
            var response = await EventBus.SendCommand(command);
            if (response.HasFails)
                return BadRequest(response.Fails);

            return Ok();
        }

    }
}
