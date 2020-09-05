using DroneDelivery.Application.Dtos.Pedido;
using System.Collections.Generic;

namespace DroneDelivery.Api.Tests.Dtos.Pedidos
{
    public class PedidosTestDto
    {

        public ICollection<PedidoDto> Pedidos { get; set; }
    }
}
