using DroneDelivery.Api.Tests.Dtos.Token;
using DroneDelivery.Application.Commands.Usuarios;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DroneDelivery.Api.Tests.Config
{
    [CollectionDefinition(nameof(IntegrationTestsFixtureCollection))]
    public class IntegrationTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<Startup>>
    { }
    public class IntegrationTestsFixture<TStartup> : IDisposable where TStartup : class
    {
        public string Token;

        public readonly DroneAppFactory<TStartup> Factory;
        public HttpClient Client;

        public IntegrationTestsFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                BaseAddress = new Uri("http://localhost")
            };

            Factory = new DroneAppFactory<TStartup>();
            Client = Factory.CreateClient(clientOptions);
        }

        public async Task RealizarLoginApi()
        {
            var userCommand = new LoginUsuarioCommand("admin@test.com", "123");

            var response = await Client.PostAsJsonAsync("/api/usuarios/login", userCommand);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();
            var jwt = JsonConvert.DeserializeObject<JsonWebTokenTestDto>(data);

            Token = jwt.Jwt.Token;
        }

        public void Dispose()
        {
            Factory.Dispose();
            Client.Dispose();
        }
    }
}
