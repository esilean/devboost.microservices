using DroneDelivery.Shared.Domain.Core.Domain;
using Flunt.Notifications;
using MediatR;

namespace DroneDelivery.Shared.Domain.Core.Queries
{
    public abstract class Query : Notifiable, IRequest<ResponseResult>
    {
    }
}
