using DroneDelivery.Shared.Domain.Core.Domain;

namespace DroneDelivery.Data.Repositorios.Interfaces
{
    public interface IRepository<T> where T : IAggregateRoot
    {
    }
}
