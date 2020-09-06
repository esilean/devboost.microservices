using DroneDelivery.Domain.Enum;
using DroneDelivery.Shared.Domain.Core.Enums;
using System;

namespace DroneDelivery.Api.Tests.Dtos.Pedidos
{
    public class CriarRepostaPagamentoDtoTests
    {
        public Guid Id { get; set; }
        public PedidoStatus Status { get; set; }
    }
}
