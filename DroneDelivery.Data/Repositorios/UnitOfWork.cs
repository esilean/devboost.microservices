using DroneDelivery.Data.Data;
using DroneDelivery.Data.Repositorios.Interfaces;
using System.Threading.Tasks;

namespace DroneDelivery.Data.Repositorios
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly DroneDbContext _context;

        public UnitOfWork(DroneDbContext context)
        {
            _context = context;
            Pedidos = new PedidoRepository(_context);
            Drones = new DroneRepository(_context);
            Usuarios = new UsuarioRepository(_context);
        }


        public IPedidoRepository Pedidos { get; private set; }

        public IDroneRepository Drones { get; private set; }

        public IUsuarioRepository Usuarios { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
