using DroneDelivery.Application.Commands.Pedidos;
using DroneDelivery.Application.Dtos.Pedido;
using DroneDelivery.Application.Queries.Pedidos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
            var response = await Mediator.RequestQuery(new PedidosQuery());
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
        ///        "peso": 10000
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
            var response = await Mediator.SendCommand(command);
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
        ///     PUT /api/pedidos
        ///     {
        ///        "status": "aprovado"
        ///     }
        ///
        /// </remarks>        
        /// <param name="command"></param>  
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtualizarStatusPedido(AtualizarPedidoStatusCommand command)
        {
            var response = await Mediator.SendCommand(command);
            if (response.HasFails)
                return BadRequest(response.Fails);
            return Ok();
        }


    }
}
