using DroneDelivery.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DroneDelivery.Data.Repositorios.Interfaces
{
    public interface IDroneRepository : IRepository<Drone>
    {
        Task<IEnumerable<Drone>> ObterTodosAsync();

        Task<IEnumerable<Drone>> ObterDronesParaEntregaAsync();

        Task AdicionarAsync(Drone drone);

    }
}
