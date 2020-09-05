using DroneDelivery.Domain.Models;
using System;
using Xunit;

namespace DroneDelivery.Domain.Tests.Models
{
    public class HistoricoPedidoTests
    {

        [Fact(DisplayName = "Criar Histórico do Pedido")]
        public void PedidoHistorico_CriarHistorico_DeveRetornarHistorico()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var droneId = Guid.NewGuid();

            //Act
            var historico = HistoricoPedido.Criar(droneId, pedidoId);

            // Assert
            Assert.Equal(pedidoId, historico.PedidoId);
            Assert.Equal(droneId, historico.DroneId);
            Assert.Null(historico.DataEntrega);
        }

        [Fact(DisplayName = "Marca entrega realizada")]
        public void PedidoHistorico_MarcarEntregaRealizada_DeveAdicionarDataEntrega()
        {
            // Arrange
            var historico = HistoricoPedido.Criar(Guid.NewGuid(), Guid.NewGuid());

            //Act
            historico.MarcarEntregaCompleta();

            // Assert
            Assert.NotNull(historico.DataEntrega);
        }
    }
}
