using DroneDelivery.Application.Interfaces;
using DroneDelivery.Application.Queries.Pedidos;
using DroneDelivery.Application.QueryHandlers.Pedidos;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Models;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DroneDelivery.Application.Tests.QueryHandlers.Pedidos
{
    public class ListarPedidosHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly ListarPedidosHandler _handler;

        public ListarPedidosHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<ListarPedidosHandler>();
        }

        [Fact(DisplayName = "Deve retornar uma lista de pedidos")]
        public async Task Pedido_SolicitarTodosPedidosDoUsuario_RetornarListaDePedidos()
        {
            // Arrange
            var query = new PedidosQuery();
            var usuario = Usuario.Criar("test", "test@test.com", 0, 0, UsuarioRole.Cliente);
            IEnumerable<Pedido> pedidos = new List<Pedido>
            {
                Pedido.Criar(1000, 1000, usuario)
            };

            _mocker.GetMock<IUsuarioAutenticado>()
                           .Setup(p => p.GetCurrentId())
                           .Returns(usuario.Id);

            _mocker.GetMock<IUnitOfWork>()
                        .Setup(p => p.Pedidos.ObterDoClienteAsync(usuario.Id))
                        .Returns(Task.FromResult(pedidos));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.HasFails);
            _mocker.GetMock<IUsuarioAutenticado>().Verify(o => o.GetCurrentId(), Times.Once);
            _mocker.GetMock<IUnitOfWork>().Verify(o => o.Pedidos.ObterDoClienteAsync(usuario.Id), Times.Once);
        }
    }
}
