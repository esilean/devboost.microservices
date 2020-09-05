using DroneDelivery.Application.Dtos.Drone;
using System.Collections.Generic;

namespace DroneDelivery.Api.Tests.Dtos.Drones
{
    public class DroneSituacaoTestDto
    {
        public ICollection<DroneSituacaoDto> Drones { get; set; }
    }
}
