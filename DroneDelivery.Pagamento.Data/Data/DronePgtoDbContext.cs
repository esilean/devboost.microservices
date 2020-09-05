using DroneDelivery.Pagamento.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DroneDelivery.Pagamento.Data.Data
{
    public class DronePgtoDbContext : DbContext
    {

        public DronePgtoDbContext(DbContextOptions<DronePgtoDbContext> options) : base(options)
        {
        }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoPagamento> Pagamentos { get; set; }

    }
}
