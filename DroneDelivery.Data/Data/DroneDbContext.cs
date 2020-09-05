using DroneDelivery.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DroneDelivery.Data.Data
{
    [ExcludeFromCodeCoverage]
    public class DroneDbContext : DbContext
    {
        public DroneDbContext() { }

        public DroneDbContext(DbContextOptions<DroneDbContext> options) : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Drone> Drones { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<HistoricoPedido> HistoricoPedidos { get; set; }

    }
}
