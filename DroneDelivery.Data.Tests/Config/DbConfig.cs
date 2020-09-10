using DroneDelivery.Data.Data;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Data.Common;

namespace DroneDelivery.Data.Tests.Config
{
    public class DbConfig : IDisposable
    {
        protected DbContextOptions<DroneDbContext> ContextOptions { get; }
        private readonly DbConnection _connection;

        public DbConfig()
        {
            ContextOptions = new DbContextOptionsBuilder<DroneDbContext>()
                .UseSqlite(CreateInMemoryDB())
                .Options;

            _connection = RelationalOptionsExtension.Extract(ContextOptions).Connection;

            Seed();
        }

        private static DbConnection CreateInMemoryDB()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }

        public void Dispose() => _connection.Dispose();

        private void Seed()
        {
            using var context = new DroneDbContext(ContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var usuario = Usuario.Criar("test", "test@test.com", -23.35566, -46.36554, UsuarioRole.Cliente);
            usuario.AdicionarPassword("123");
            usuario.AdicionarRefreshToken("refreshtoken", DateTime.Now.AddDays(1));
            context.Add(usuario);

            var drone1 = Drone.Criar(12000, 3, 35, 100, DroneStatus.Livre);
            var drone2 = Drone.Criar(10000, 4, 35, 50, DroneStatus.Livre);

            context.Add(drone1);
            context.Add(drone2);

            var pedido = Pedido.Criar(5000, 1000, usuario);

            context.Add(pedido);

            context.SaveChanges();
        }

    }
}
