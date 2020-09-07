using DroneDelivery.Application.Commands.Pedidos;
using DroneDelivery.Application.Dtos.Pedido;
using DroneDelivery.Application.Queries.Pedidos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DroneDelivery.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : BaseController
    {

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PedidoDto>>> ObterTodos()
        {
            var response = await EventBus.RequestQuery(new PedidosQuery());
            if (response.HasFails)
                return BadRequest(response.Fails);

            return Ok(response.Data);
        }


        /// <summary>
        /// Criar um pedido
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/pedidos
        ///     {
        ///        "peso": 10000,
        ///        "valor": 999
        ///     }
        ///
        /// </remarks>        
        /// <param name="command"></param>  
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Adicionar(CriarPedidoCommand command)
        {
            var response = await EventBus.SendCommand(command);
            if (response.HasFails)
                return BadRequest(response.Fails);

            return Ok();
        }


        /// <summary>
        /// Atualizar o status do pedido
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/pedidos/atualizarstatus
        ///     {
        ///        "id": "2e4f41f4-dfd2-4464-8896-d39e22315cdd",
        ///        "status": 3
        ///     }
        ///
        /// </remarks>        
        /// <param name="command"></param>  
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("atualizarstatus")]
        public async Task<IActionResult> AtualizarStatusPedido(AtualizarPedidoStatusCommand command)
        {
            var response = await EventBus.SendCommand(command);
            if (response.HasFails)
                return BadRequest(response.Fails);
            return Ok();
        }


    }
}
