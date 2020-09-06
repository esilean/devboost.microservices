using DroneDelivery.Application.Commands.Usuarios;
using DroneDelivery.Application.Interfaces;
using DroneDelivery.Data.Repositorios.Interfaces;
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
    public class LoginUsuarioHandler : ValidatorResponse, IRequestHandler<LoginUsuarioCommand, ResponseResult>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IGeradorToken _geradorToken;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public LoginUsuarioHandler(IUnitOfWork unitOfWork, IGeradorToken geradorToken, IPasswordHasher<Usuario> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _geradorToken = geradorToken;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResponseResult> Handle(LoginUsuarioCommand request, CancellationToken cancellationToken)
        {
            request.Validate();
            if (request.Notifications.Any())
            {
                _response.AddNotifications(request.Notifications);
                return _response;
            }

            // Recupera o usuário
            var usuario = await _unitOfWork.Usuarios.ObterPorEmailAsync(request.Email);
            if (usuario == null)
            {
                _response.AddNotification(new Notification("usuario", Erros.ErroCliente_UsuarioOuSenhaInvalido));
                return _response;
            }

            var passwordResult = _passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, request.Password);
            if (passwordResult == PasswordVerificationResult.Failed)
            {
                _response.AddNotification(new Notification("usuario", Erros.ErroCliente_UsuarioOuSenhaInvalido));
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
