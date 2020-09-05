using DroneDelivery.Data.Data;
using DroneDelivery.Data.Repositorios;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Data.Tests.Config;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DroneDelivery.Data.Tests.Repositorios
{
    public class DroneRepositoryTests : DbConfig
    {
        private readonly IDroneRepository _droneRepository;
        private readonly DroneDbContext _context;

        public DroneRepositoryTests()
        {
            _context = new DroneDbContext(ContextOptions);
            _droneRepository = new DroneRepository(_context);
        }

        [Fact(DisplayName = "Salvar drone no banco de dados")]
        public async Task Drone_QuandoSalvarDrone_DeveExistirNoBancoDados()
        {
            //Arrange
            var drone = Drone.Criar(8000, 1, 20, 50, DroneStatus.Livre);

            //Act
            await _droneRepository.AdicionarAsync(drone);
            await _context.SaveChangesAsync();

            //Assert
            Assert.True(_context.Set<Drone>().Any(x => x.Capacidade == 8000));
            Assert.True(_context.Set<Drone>().Any(x => x.Velocidade == 1));
            Assert.True(_context.Set<Drone>().Any(x => x.Autonomia == 20));
            Assert.True(_context.Set<Drone>().Any(x => x.Carga == 50));
        }

        [Fact(DisplayName = "Obter drones do banco de dados")]
        public async Task Drone_QuandoExistiremDrones_DeveRetornarDoBancoDados()
        {
            //Arrage
            //Act
            var drones = await _droneRepository.ObterTodosAsync();

            //Assert
            Assert.True(drones.Count() > 0);
            Assert.True(_context.Set<Drone>().Any(x => x.Capacidade == 12000));
        }

    }
}
