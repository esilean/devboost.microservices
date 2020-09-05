using DroneDelivery.Application.Queries.Usuarios;
using DroneDelivery.Application.QueryHandlers.Usuarios;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Models;
using Moq;
using Moq.AutoMock;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DroneDelivery.Application.Tests.QueryHandlers.Usuarios
{
    public class ListarUsuariosHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly ListarUsuariosHandler _handler;

        public ListarUsuariosHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<ListarUsuariosHandler>();
        }

        [Fact(DisplayName = "Deve retornar uma lista de usuários")]
        public async Task Usuario_SolicitarTodosUsuarios_RetornarListaDeUsuarios()
        {
            // Arrange
            var query = new UsuariosQuery();

            IEnumerable<Usuario> drones = new List<Usuario>
            {
                Usuario.Criar("test", "test@test.com", 1, 1, UsuarioRole.Admin)
            };

            _mocker.GetMock<IUnitOfWork>()
                        .Setup(p => p.Usuarios.ObterTodosAsync())
                        .Returns(Task.FromResult(drones));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.HasFails);
            _mocker.GetMock<IUnitOfWork>().Verify(o => o.Usuarios.ObterTodosAsync(), Times.Once);
        }
    }
}
