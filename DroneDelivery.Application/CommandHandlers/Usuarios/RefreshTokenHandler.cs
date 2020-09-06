using DroneDelivery.Application.Commands.Usuarios;
using DroneDelivery.Application.Interfaces;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Shared.Domain.Core.Domain;
using DroneDelivery.Shared.Domain.Core.Validator;
using DroneDelivery.Shared.Utility.Messages;
using Flunt.Notifications;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DroneDelivery.Application.CommandHandlers.Usuarios
{
    public class RefreshTokenHandler : ValidatorResponse, IRequestHandler<RefreshTokenCommand, ResponseResult>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IGeradorToken _geradorToken;
        public RefreshTokenHandler(IUnitOfWork unitOfWork, IGeradorToken geradorToken)
        {
            _geradorToken = geradorToken;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            request.Validate();
            if (request.Notifications.Any())
            {
                _response.AddNotifications(request.Notifications);
                return _response;
            }

            var usuario = await _unitOfWork.Usuarios.ObterPorEmailAsync(request.Email);
            if (usuario == null
                || usuario.RefreshToken != request.RefreshToken
                || usuario.RefreshTokenExpiracao < DateTime.UtcNow)
            {
                _response.AddNotification(new Notification("usuario", Erros.ErroCliente_RefreshTokenNaoAutorizado));
                return _response;
            }

            var jwt = _geradorToken.GerarToken(usuario);
            usuario.AdicionarRefreshToken(jwt.RefreshToken.Token, jwt.RefreshToken.DataExpiracao);
            await _unitOfWork.SaveAsync();

            _response.AddValue(new
            {
                email = usuario.Email,
                jwt
            });

            return _response;
        }
    }
}
