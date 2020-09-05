using DroneDelivery.Data.Data;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneDelivery.Data.Repositorios
{
    public class DroneRepository : IDroneRepository
    {
        private readonly DroneDbContext _context;

        public DroneRepository(DroneDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Drone drone)
        {
            await _context.Drones.AddAsync(drone);
        }

        public async Task<IEnumerable<Drone>> ObterTodosAsync()
        {
            return await _context.Drones.Include(x => x.Pedidos).ToListAsync();
        }

        public async Task<IEnumerable<Drone>> ObterDronesParaEntregaAsync()
        {
            var drones = await _context.Drones.Include(x => x.Pedidos).ThenInclude(x => x.Usuario)
                .Where(x => x.Status == DroneStatus.Livre
                    && x.Pedidos.Count() > 0).ToListAsync();

            return drones;
        }
    }
}
