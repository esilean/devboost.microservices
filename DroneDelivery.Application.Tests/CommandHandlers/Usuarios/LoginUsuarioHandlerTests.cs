using DroneDelivery.Application.CommandHandlers.Usuarios;
using DroneDelivery.Application.Commands.Usuarios;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Models;
using Moq.AutoMock;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DroneDelivery.Application.Tests.CommandHandlers.Usuarios
{
    public class LoginUsuarioHandlerTests
    {

        private readonly AutoMocker _mocker;
        private readonly LoginUsuarioHandler _handler;

        public LoginUsuarioHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<LoginUsuarioHandler>();
        }

        [Fact(DisplayName = "Não deve retornar um token para o usuario com command invalido")]
        public async Task Login_AoRealizarLoginComComandoInvalido_RetornarNotificacoesComFalha()
        {
            // Arrange
            var command = new LoginUsuarioCommand("", "");

            // Act
            var responseResult = await _handler.Handle(command, CancellationToken.None);


            // Assert
            Assert.True(responseResult.HasFails);
        }

        [Fact(DisplayName = "Não deve retornar um token para o usuario que nao exista")]
        public async Task Login_AoRealizarLoginComEmailQueNaoExiste_RetornarNotificacoesComFalha()
        {
            // Arrange
            var command = new LoginUsuarioCommand("naoexiste@test.com", "Admin123!");

            //Obter usuario
            _mocker.GetMock<IUnitOfWork>()
                        .Setup(p => p.Usuarios.ObterPorEmailAsync(command.Email));

            // Act
            var responseResult = await _handler.Handle(command, CancellationToken.None);


            // Assert
            Assert.True(responseResult.HasFails);
        }

        [Fact(DisplayName = "Não deve retornar um token para o usuario que nao utiliza senha invalida")]
        public async Task Login_AoRealizarLoginComSenhaInvalida_RetornarNotificacoesComFalha()
        {
            // Arrange
            var command = new LoginUsuarioCommand("admin@test.com", "senhaerrada");

            var usuario = Usuario.Criar("test", "test@test.com", 0, 0, UsuarioRole.Cliente);
            usuario.AdicionarPassword("teste");

            //Obter usuario
            _mocker.GetMock<IUnitOfWork>()
                        .Setup(p => p.Usuarios.ObterPorEmailAsync(command.Email))
                        .Returns(Task.FromResult(usuario));

            // Act
            var responseResult = await _handler.Handle(command, CancellationToken.None);


            // Assert
            Assert.True(responseResult.HasFails);
        }

    }
}
