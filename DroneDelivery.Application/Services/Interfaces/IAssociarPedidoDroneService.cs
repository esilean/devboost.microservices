using DroneDelivery.Domain.Models;
using System.Threading.Tasks;

namespace DroneDelivery.Application.Services.Interfaces
{
    public interface IAssociarPedidoDroneService
    {
        Task<Drone> AssociarPedidoAoDrone(Pedido pedido);
    }
}
