using DroneDelivery.Data.Data;
using DroneDelivery.Data.Repositorios;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Data.Tests.Config;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DroneDelivery.Data.Tests.Repositorios
{
    public class PedidoRepositoryTests : DbConfig
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly DroneDbContext _context;

        public PedidoRepositoryTests()
        {
            _context = new DroneDbContext(ContextOptions);
            _pedidoRepository = new PedidoRepository(_context);
        }

        [Fact(DisplayName = "Salvar pedido no banco de dados")]
        public async Task Pedido_QuandoSalvarPedido_DeveExistirNoBancoDados()
        {
            //Arrange
            var usuario = Usuario.Criar("test", "test@email", 0, 0, UsuarioRole.Cliente);
            usuario.AdicionarPassword("password");
            usuario.AdicionarRefreshToken("refreshToken", DateTime.Now.AddDays(1));
            await _context.AddAsync(usuario);

            var pedido = Pedido.Criar(999, 1000, usuario);

            //Act
            await _pedidoRepository.AdicionarAsync(pedido);
            await _context.SaveChangesAsync();

            //Assert
            Assert.True(_context.Set<Pedido>().Any(x => x.Peso == 999));
            Assert.True(_context.Set<Pedido>().Any(x => x.UsuarioId == pedido.UsuarioId));
        }

        [Fact(DisplayName = "Obter pedidos do cliente do banco de dados")]
        public async Task Pedido_QuandoExistiremPedidos_DeveRetornarDoBancoDados()
        {
            //Arrange
            var usuario = Usuario.Criar("test", "test@email", 0, 0, UsuarioRole.Cliente);
            usuario.AdicionarPassword("password");
            usuario.AdicionarRefreshToken("refreshToken", DateTime.Now.AddDays(1));
            await _context.AddAsync(usuario);

            var pedido1 = Pedido.Criar(123, 1000, usuario);
            var pedido2 = Pedido.Criar(456, 1000, usuario);

            await _pedidoRepository.AdicionarAsync(pedido1);
            await _pedidoRepository.AdicionarAsync(pedido2);
            await _context.SaveChangesAsync();


            //Act
            var pedidos = await _pedidoRepository.ObterDoClienteAsync(usuario.Id);

            //Assert
            Assert.NotNull(pedidos.FirstOrDefault(x => x.Peso == 123));
            Assert.NotNull(pedidos.FirstOrDefault(x => x.Peso == 456));
        }

        [Fact(DisplayName = "Obter histórico pedidos do drone do banco de dados")]
        public async Task Pedido_QuandoExistiremHistoricosPedidosEmUmDrone_DeveRetornarDoBancoDados()
        {
            //Arrange
            var usuario = Usuario.Criar("test", "test@email", 0, 0, UsuarioRole.Cliente);
            usuario.AdicionarPassword("password");
            usuario.AdicionarRefreshToken("refreshToken", DateTime.Now.AddDays(1));
            await _context.AddAsync(usuario);

            var drone = Drone.Criar(12000, 5, 40, 100, DroneStatus.EmEntrega);
            await _context.AddAsync(drone);

            var pedido = Pedido.Criar(123, 1000, usuario);
            pedido.AssociarDrone(drone);
            await _context.AddAsync(pedido);

            var historicoPedido = HistoricoPedido.Criar(drone.Id, pedido.Id);
            await _context.AddAsync(historicoPedido);

            await _context.SaveChangesAsync();


            //Act
            var historicoDrone = await _pedidoRepository.ObterHistoricoPedidosDoDroneAsync(drone.Id);

            //Assert
            Assert.True(historicoDrone.Count(x => x.DroneId == drone.Id) == 1);
        }

    }
}
