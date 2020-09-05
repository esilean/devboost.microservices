using DroneDelivery.Domain.Core.Domain;
using Flunt.Notifications;
using MediatR;

namespace DroneDelivery.Domain.Core.Events
{
    public abstract class Message : Notifiable, IRequest<ResponseResult>
    {
        public string MessageType { get; protected set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}
