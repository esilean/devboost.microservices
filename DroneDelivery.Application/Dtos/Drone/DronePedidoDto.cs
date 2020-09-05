using DroneDelivery.Application.Dtos.Usuario;
using Newtonsoft.Json;
using System;

namespace DroneDelivery.Application.Dtos.Drone
{
    public class DronePedidoDto
    {
        public Guid Id { get; set; }

        [JsonProperty("cliente")]
        public UsuarioDto Usuario { get; set; }

    }
}
