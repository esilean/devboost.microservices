using DroneDelivery.Api.Configs;
using DroneDelivery.Application.Commands.Usuarios;
using DroneDelivery.Application.Dtos.Usuario;
using DroneDelivery.Application.Queries.Usuarios;
using DroneDelivery.Shared.Domain.Core.Domain;
using Flunt.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DroneDelivery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : BaseController
    {
        private readonly IConfiguration _configuration;

        public UsuariosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> ObterTodos()
        {
            var response = await EventBus.RequestQuery(new UsuariosQuery());
            if (response.HasFails)
                return BadRequest(response.Fails);

            return Ok(response.Data);
        }


        /// <summary>
        /// Logar um usuário
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/usuarios/login
        ///     {
        ///         "email": "test@test.com",
        ///         "password": "123",
        ///     } 
        ///
        /// </remarks>        
        /// <param name="command"></param>  
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IReadOnlyCollection<Notification>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseResult>> Login([FromBody] LoginUsuarioCommand command)
        {
            var response = await EventBus.SendCommand(command);
            if (response.HasFails)
                return BadRequest(response.Fails);

            return Ok(response.Data);
        }


        /// <summary>
        /// Criar um usuário
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/usuarios/registrar
        ///     {
        ///         "email": "test@test.com",
        ///         "nome": "Test",
        ///         "password": "123",
        ///         "latitude": -23.5950753,
        ///         "longitude": -46.645421
        ///     }
        ///
        /// </remarks>        
        /// <param name="command"></param>  
        [HttpPost("registrar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IReadOnlyCollection<Notification>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody] CriarUsuarioCommand command)
        {
            var response = await EventBus.SendCommand(command);
            if (response.HasFails)
                return BadRequest(response.Fails);

            return Ok();
        }

        /// <summary>
        /// Atualizar token expirado
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/usuarios/refresh
        ///     {
        ///         "email": "test@test.com",
        ///         "token": "123",
        ///         "refreshToken": "123",
        ///     } 
        ///
        /// </remarks>        
        /// <param name="command"></param>  
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IReadOnlyCollection<Notification>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseResult>> Refresh([FromBody] RefreshTokenCommand command)
        {
            var principal = ObterClaimsTokenExpirado(command.Token);
            var email = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            if (email != command.Email)
            {
                var responseEmail = new ResponseResult();
                responseEmail.AddNotification(new Notification("refresh_token", "email não autorizado"));
                return BadRequest(responseEmail.Fails);
            }

            var response = await EventBus.SendCommand(command);
            if (response.HasFails)
                return BadRequest(response.Fails);

            return Ok(response.Data);
        }

        private ClaimsPrincipal ObterClaimsTokenExpirado(string token)
        {
            var tokenValidationParameters = Token.TokenParametersConfig(_configuration["JwtSettings:SigningKey"], false);
            return Token.ValidarTokenExpirado(token, tokenValidationParameters);
        }

    }
}
