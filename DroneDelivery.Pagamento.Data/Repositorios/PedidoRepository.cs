using DroneDelivery.Pagamento.Data.Data;
using DroneDelivery.Pagamento.Data.Repositorios.Interfaces;
using DroneDelivery.Pagamento.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Data.Repositorios
{
    public class PedidoRepository : IPedidoRepository
    {

        private readonly DronePgtoDbContext _context;

        public PedidoRepository(DronePgtoDbContext context)
        {
            _context = context;
        }


        public async Task AdicionarAsync(Pedido pedido)
        {
            await _context.AddAsync(pedido);
        }

        public async Task AdicionarPagamentoAsync(PedidoPagamento pedidoPagamento)
        {
            await _context.Pagamentos.AddAsync(pedidoPagamento);
        }

        public async Task<Pedido> ObterPorIdAsync(Guid id)
        {
            return await _context.Pedidos.Include(x => x.Pagamentos).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
