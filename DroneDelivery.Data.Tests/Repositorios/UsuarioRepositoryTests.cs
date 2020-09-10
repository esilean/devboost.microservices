using DroneDelivery.Data.Data;
using DroneDelivery.Data.Repositorios;
using DroneDelivery.Data.Tests.Config;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DroneDelivery.Data.Tests.Repositorios
{
    public class UsuarioRepositoryTests : DbConfig
    {

        private readonly UsuarioRepository _usuarioRepository;
        private readonly DroneDbContext _context;

        public UsuarioRepositoryTests()
        {
            _context = new DroneDbContext(ContextOptions);
            _usuarioRepository = new UsuarioRepository(_context);
        }

        [Fact(DisplayName = "Salvar usuário admin no banco de dados")]
        public async Task UsuarioAdmin_QuandoSalvarUsuario_DeveExistirNoBancoDados()
        {
            //Arrange
            var usuario = Usuario.Criar("admin", "admin@test.com", -23, -46, UsuarioRole.Admin);
            usuario.AdicionarPassword("password");
            usuario.AdicionarRefreshToken("refreshToken", DateTime.Now.AddDays(1));

            //Act
            await _usuarioRepository.AdicionarAsync(usuario);
            await _context.SaveChangesAsync();

            //Assert
            Assert.True(_context.Set<Usuario>().Any(x => x.Id == usuario.Id));
            Assert.True(_context.Set<Usuario>().Any(x => (int)x.Role == (int)usuario.Role));
        }

        [Fact(DisplayName = "Salvar usuário cliente no banco de dados")]
        public async Task UsuarioCliente_QuandoSalvarUsuario_DeveExistirNoBancoDados()
        {
            //Arrange
            var usuario = Usuario.Criar("admin", "admin@test.com", -23, -46, UsuarioRole.Cliente);
            usuario.AdicionarPassword("password");
            usuario.AdicionarRefreshToken("refreshToken", DateTime.Now.AddDays(1));

            //Act
            await _usuarioRepository.AdicionarAsync(usuario);
            await _context.SaveChangesAsync();

            //Assert
            Assert.True(_context.Set<Usuario>().Any(x => x.Id == usuario.Id));
            Assert.True(_context.Set<Usuario>().Any(x => (int)x.Role == (int)usuario.Role));
        }

        [Fact(DisplayName = "Obter usuários do banco de dados")]
        public async Task Usuario_QuandoExistiremUsuarios_DeveRetornarDoBancoDados()
        {
            //Arrange
            //Act
            var usuarios = await _usuarioRepository.ObterTodosAsync();

            //Assert
            Assert.True(usuarios.Count() > 0);
        }

    }
}
