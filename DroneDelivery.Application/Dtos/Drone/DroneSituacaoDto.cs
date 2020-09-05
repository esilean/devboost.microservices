using System;
using System.Collections.Generic;

namespace DroneDelivery.Application.Dtos.Drone
{
    public class DroneSituacaoDto
    {
        public Guid Id { get; set; }

        public string Situacao { get; set; }

        public IEnumerable<DronePedidoDto> Pedidos { get; set; }

    }
}
