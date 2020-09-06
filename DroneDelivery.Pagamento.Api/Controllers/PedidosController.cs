using DroneDelivery.Pagamento.Application.Commands.Pedidos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Api.Controllers
{

    public class PedidosController : BaseController
    {


        /// <summary>
        /// Receber um pedido para pagamento
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/pedidos
        ///     {
        ///        "id": Guid
        ///        "valor": 999
        ///     }
        ///
        /// </remarks>        
        /// <param name="command"></param>  
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Criar(CriarPedidoCommand command)
        {
            var response = await EventBus.SendCommand(command);
            if (response.HasFails)
                return BadRequest(response.Fails);

            return Ok();
        }

    }
}
