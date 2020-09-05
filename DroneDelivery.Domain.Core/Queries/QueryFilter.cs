using DroneDelivery.Domain.Core.Domain;
using Flunt.Notifications;
using MediatR;

namespace DroneDelivery.Domain.Core.Queries
{
    public class QueryFilter : Notifiable, IRequest<ResponseResult>
    {
    }
}
