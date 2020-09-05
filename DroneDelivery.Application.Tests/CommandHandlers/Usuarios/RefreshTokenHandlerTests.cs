using DroneDelivery.Application.CommandHandlers.Usuarios;
using DroneDelivery.Application.Commands.Usuarios;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Models;
using Moq.AutoMock;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DroneDelivery.Application.Tests.CommandHandlers.Usuarios
{
    public class RefreshTokenHandlerTests
    {

        private readonly AutoMocker _mocker;
        private readonly RefreshTokenHandler _handler;

        public RefreshTokenHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<RefreshTokenHandler>();
        }

        [Fact(DisplayName = "Não deve gerar um novo token com command invalido")]
        public async Task RefreshToken_AoSolicitarUmNovoTokenComComandoInvalido_RetornarNotificacoesComFalha()
        {
            // Arrange
            var command = new RefreshTokenCommand("", "", "");

            // Act
            var responseResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(responseResult.HasFails);
            Assert.Equal(4, responseResult.Fails.Count());
        }

        [Fact(DisplayName = "Não deve gerar um novo token com refreshToken invalido")]
        public async Task RefreshToken_AoSolicitarUmNovoTokenComRefreshTokenInvalido_RetornarNotificacoesComFalha()
        {

            // Arrange
            var command = new RefreshTokenCommand("admin@test.com", "123", "123");

            //Obter usuario
            var usuarioId = Guid.NewGuid();
            var usuario = Usuario.Criar("test", "admin@test.com", 0, 0, UsuarioRole.Cliente);
            usuario.AdicionarRefreshToken("invalido", DateTime.Now);

            _mocker.GetMock<IUnitOfWork>()
                    .Setup(p => p.Usuarios.ObterPorEmailAsync(usuario.Email))
                    .Returns(Task.FromResult(usuario));

            // Act
            var responseResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(responseResult.HasFails);
        }

    }
}
