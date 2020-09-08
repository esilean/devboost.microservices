using DroneDelivery.Pagamento.Application.Commands.Pagamentos;
using DroneDelivery.Shared.Domain.Core.Bus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Api.Controllers
{

    public class PagamentosController : BaseController
    {

        public PagamentosController(IEventBus eventBus) : base(eventBus)
        {
        }

        /// <summary>
        /// Realizar o pagamento de um pedido
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/pagamentos
        ///     {
        ///        "numeroCartao": "4242424242424242",
        ///        "vencimentoCartao": "2025-12-31",
        ///        "codigoSeguranca": 123
        ///     }
        ///
        /// </remarks>        
        /// <param name="pedidoId"></param>  
        /// <param name="command"></param>  
        [HttpPost("{pedidoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RealizarPagamento(Guid pedidoId, CriarPagamentoCommand command)
        {
            command.MarcarPedidoId(pedidoId);
            var response = await EventBus.SendCommand(command);
            if (response.HasFails)
                return BadRequest(response.Fails);

            return Ok();
        }

        /// <summary>
        /// Webhook para receber reposta do gateway de pagamento
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/pagamentos/webhook
        ///     {
        ///        "pedidoId": "2e4f41f4-dfd2-4464-8896-d39e22315cdd",
        ///        "status": 1
        ///     }
        ///
        /// </remarks>        
        /// <param name="command"></param>  
        [HttpPost("webhook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Webhook(WebhookPagamentoCommand command)
        {
            var response = await EventBus.SendCommand(command);
            if (response.HasFails)
                return BadRequest(response.Fails);

            return Ok();
        }

    }
}
