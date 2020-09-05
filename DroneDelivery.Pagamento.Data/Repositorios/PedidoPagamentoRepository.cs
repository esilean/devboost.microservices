using DroneDelivery.Pagamento.Data.Data;
using DroneDelivery.Pagamento.Data.Repositorios.Interfaces;
using DroneDelivery.Pagamento.Domain.Models;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Data.Repositorios
{
    public class PedidoPagamentoRepository : IPedidoPagamentoRepository
    {

        private readonly DronePgtoDbContext _context;

        public PedidoPagamentoRepository(DronePgtoDbContext context)
        {
            _context = context;
        }


        public async Task AdicionarAsync(PedidoPagamento pedidoPagamento)
        {
            await _context.AddAsync(pedidoPagamento);
        }
    }
}
