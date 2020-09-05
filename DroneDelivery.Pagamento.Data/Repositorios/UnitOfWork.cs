using DroneDelivery.Pagamento.Data.Data;
using DroneDelivery.Pagamento.Data.Repositorios.Interfaces;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Data.Repositorios
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly DronePgtoDbContext _context;

        public UnitOfWork(DronePgtoDbContext context)
        {
            _context = context;
            PedidoPagamentos = new PedidoPagamentoRepository(_context);
        }


        public IPedidoPagamentoRepository PedidoPagamentos { get; }

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
