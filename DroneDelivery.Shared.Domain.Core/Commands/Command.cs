using DroneDelivery.Shared.Domain.Core.Events;
using System;

namespace DroneDelivery.Shared.Domain.Core.Commands
{
    public abstract class Command : Message
    {
        public DateTime Timestamp { get; protected set; }
        protected Command()
        {
            Timestamp = DateTime.Now;
        }
    }
}
