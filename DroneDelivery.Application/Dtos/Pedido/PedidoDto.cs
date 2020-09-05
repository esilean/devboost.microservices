using DroneDelivery.Application.Dtos.Drone;
using DroneDelivery.Application.Dtos.Usuario;
using Newtonsoft.Json;
using System;

namespace DroneDelivery.Application.Dtos.Pedido
{
    public class PedidoDto
    {
        public Guid Id { get; set; }

        public double Peso { get; set; }

        public DroneDto Drone { get; set; }

        [JsonProperty("cliente")]
        public UsuarioDto Usuario { get; set; }
    }
}
