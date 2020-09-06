using DroneDelivery.Application.Commands.Usuarios;
using DroneDelivery.Application.Interfaces;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Models;
using DroneDelivery.Shared.Domain.Core.Domain;
using DroneDelivery.Shared.Domain.Core.Validator;
using DroneDelivery.Shared.Utility.Messages;
using Flunt.Notifications;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DroneDelivery.Application.CommandHandlers.Usuarios
{
    public class CriarUsuarioHandler : ValidatorResponse, IRequestHandler<CriarUsuarioCommand, ResponseResult>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly IGeradorToken _geradorToken;

        public CriarUsuarioHandler(IUnitOfWork unitOfWork, IPasswordHasher<Usuario> passwordHasher, IGeradorToken geradorToken)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _geradorToken = geradorToken;
        }

        public async Task<ResponseResult> Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
        {
            request.Validate();
            if (request.Notifications.Any())
            {
                _response.AddNotifications(request.Notifications);
                return _response;
            }

            var usuario = await _unitOfWork.Usuarios.ObterPorEmailAsync(request.Email);
            if (usuario != null)
            {
                _response.AddNotification(new Notification("usuário", Erros.ErroCliente_EmailJaExiste));
                return _response;
            }

            usuario = Usuario.Criar(request.Nome, request.Email, request.Latitude, request.Longitude, UsuarioRole.Admin);

            //hash password
            var passwordHash = _passwordHasher.HashPassword(usuario, request.Password);
            usuario.AdicionarPassword(passwordHash);

            //tokens
            var refreshToken = _geradorToken.GerarRefreshToken(usuario);
            usuario.AdicionarRefreshToken(refreshToken.Token, refreshToken.DataExpiracao);

            await _unitOfWork.Usuarios.AdicionarAsync(usuario);
            await _unitOfWork.SaveAsync();

            return _response;
        }
    }
}
