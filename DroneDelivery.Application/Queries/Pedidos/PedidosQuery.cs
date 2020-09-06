using DroneDelivery.Shared.Domain.Core.Domain;
using DroneDelivery.Shared.Domain.Core.Queries;
using MediatR;

namespace DroneDelivery.Application.Queries.Pedidos
{
    public class PedidosQuery : Query, IRequest<ResponseResult> { }
}
