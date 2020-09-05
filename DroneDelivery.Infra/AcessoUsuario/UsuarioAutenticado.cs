using DroneDelivery.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace DroneDelivery.Infra.AcessoUsuario
{
    public class UsuarioAutenticado : IUsuarioAutenticado
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsuarioAutenticado(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetCurrentId()
        {
            var usuarioId = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x =>
                    x.Type == ClaimTypes.NameIdentifier)?.Value;

            return Guid.Parse(usuarioId);
        }
    }
}
