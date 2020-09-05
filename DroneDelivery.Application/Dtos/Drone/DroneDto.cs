using DroneDelivery.Domain.Enum;
using System;

namespace DroneDelivery.Application.Dtos.Drone
{
    public class DroneDto
    {
        public Guid Id { get; set; }
        public double Capacidade { get; set; }
        public double Velocidade { get; set; }
        public double Autonomia { get; set; }
        public double Carga { get; set; }
        public DroneStatus Status { get; set; }

    }
}
