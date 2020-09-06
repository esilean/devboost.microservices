using DroneDelivery.Application.Commands.Drones;
using DroneDelivery.Application.Dtos.Drone;
using DroneDelivery.Application.Queries.Drones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DroneDelivery.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DronesController : BaseController
    {

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DroneDto>>> ObterTodos()
        {
            var response = await EventBus.RequestQuery(new DronesQuery());
            if (response.HasFails)
                return BadRequest(response.Fails);

            return Ok(response.Data);
        }

        [HttpGet("situacao")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DroneSituacaoDto>>> ObterSituacaoDrones()
        {
            var response = await EventBus.RequestQuery(new SituacaoDronesQuery());
            if (response.HasFails)
                return BadRequest(response.Fails);

            return Ok(response.Data);
        }

        /// <summary>
        /// Criar um drone
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/drones
        ///     {
        ///         "capacidade": 12000,
        ///         "velocidade": 3.33333,
        ///         "autonomia": 35,
        ///         "carga": 60
        ///     }
        ///
        /// </remarks>        
        /// <param name="command"></param>  
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Adicionar(CriarDroneCommand command)
        {
            var response = await EventBus.SendCommand(command);
            if (response.HasFails)
                return BadRequest(response.Fails);

            return Ok();
        }

    }
}
