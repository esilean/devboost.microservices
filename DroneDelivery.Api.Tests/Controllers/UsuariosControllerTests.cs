using DroneDelivery.Api.Tests.Config;
using DroneDelivery.Api.Tests.Dtos.Token;
using DroneDelivery.Application.Commands.Usuarios;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DroneDelivery.Api.Tests.Controllers
{

    [Collection(nameof(IntegrationTestsFixtureCollection))]
    public class UsuariosControllerTests
    {

        private readonly IntegrationTestsFixture<Startup> _testsFixture;

        public UsuariosControllerTests(IntegrationTestsFixture<Startup> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Deve criar um usuário no banco de dados")]
        public async Task Usuario_CriarUsuario_Retornar200OKSucesso()
        {
            // Arrange
            var criarUsuarioCommand = new CriarUsuarioCommand("testnovo@test.com", "Maria", -23.5950753, -46.645421, "Admin123@");

            // Act
            var postResponse = await _testsFixture.Client.PostAsJsonAsync("/api/usuarios/registrar", criarUsuarioCommand);

            // Assert
            postResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Não deve criar um usuário no banco de dados se email inválido")]
        public async Task Usuario_CriarUsuarioEmailInvalido_Retornar400BadRequest()
        {
            // Arrange
            var criarUsuarioCommand = new CriarUsuarioCommand("", "Maria", -23.5950753, -46.645421, "Admin123@");

            // Act
            var postResponse = await _testsFixture.Client.PostAsJsonAsync("/api/usuarios/registrar", criarUsuarioCommand);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);
        }

        [Fact(DisplayName = "Deve retornar um token se usuario e senha OK")]
        public async Task Usuario_RealizarLogin_RetornarToken200Sucesso()
        {
            // Arrange
            var criarUsuarioCommand = new CriarUsuarioCommand("test@test.com", "Maria", -23.5950753, -46.645421, "Admin123@");

            await _testsFixture.Client.PostAsJsonAsync("/api/usuarios/registrar", criarUsuarioCommand);

            var loginUsuarioCommand = new LoginUsuarioCommand("test@test.com", "Admin123@");

            // Act
            var postResponseLogin = await _testsFixture.Client.PostAsJsonAsync("/api/usuarios/login", loginUsuarioCommand);

            // Assert
            postResponseLogin.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Deve retornar um token novo se solicitado atraves do RefreshToken")]
        public async Task Usuario_RevalidaToken_RetornarToken200Sucesso()
        {
            //Arrange
            // Criar Usuario
            var criarUsuarioCommand = new CriarUsuarioCommand("test@test.com", "Maria", -23.5950753, -46.645421, "Admin123@");
            await _testsFixture.Client.PostAsJsonAsync("/api/usuarios/registrar", criarUsuarioCommand);

            //Logar Usuario
            var loginUsuarioCommand = new LoginUsuarioCommand("test@test.com", "Admin123@");

            var response = await _testsFixture.Client.PostAsJsonAsync("/api/usuarios/login", loginUsuarioCommand);
            var data = await response.Content.ReadAsStringAsync();
            var jwtDto = JsonConvert.DeserializeObject<JsonWebTokenTestDto>(data);

            var refToken = new RefreshTokenCommand(jwtDto.Email, jwtDto.Jwt.Token, jwtDto.Jwt.RefreshToken.Token);

            // Act
            var responseRefresh = await _testsFixture.Client.PostAsJsonAsync("/api/usuarios/refresh", refToken);

            // Assert
            responseRefresh.EnsureSuccessStatusCode();

        }

        [Fact(DisplayName = "Deve retornar usuarios")]
        public async Task Usuario_RetornarUsuarios_200OKSucesso()
        {
            // Arrange
            await _testsFixture.RealizarLoginApi();
            _testsFixture.Client.AddToken(_testsFixture.Token);

            // Act
            var response = await _testsFixture.Client.GetAsync("/api/usuarios");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
