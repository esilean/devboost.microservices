using DroneDelivery.Data.Data;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneDelivery.Data.Repositorios
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly DroneDbContext _context;

        public PedidoRepository(DroneDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
        }

        public async Task<IEnumerable<Pedido>> ObterDoClienteAsync(Guid usuarioId)
        {
            return await _context.Pedidos.Include(x => x.Drone).Include(x => x.Usuario).Where(x => x.UsuarioId == usuarioId).ToListAsync();
        }


        public async Task CriarHistoricoPedidoAsync(IEnumerable<Pedido> pedidos)
        {
            foreach (var pedido in pedidos)
                await _context.HistoricoPedidos.AddAsync(HistoricoPedido.Criar(pedido.DroneId.GetValueOrDefault(), pedido.Id));
        }

        public async Task<IEnumerable<HistoricoPedido>> ObterHistoricoPedidosDoDroneAsync(Guid droneId)
        {
            return await _context.HistoricoPedidos.Where(x => x.DroneId == droneId).ToListAsync();
        }

        public async Task<Pedido> ObterPorIdAsync(Guid pedidoId)
        {
            return await _context.Pedidos.FirstOrDefaultAsync(x => x.Id == pedidoId);
        }
    }
}
