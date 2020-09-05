using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;

namespace DroneDelivery.Pagamento.Api.Tests.Config
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

        public void Dispose()
        {
            Factory.Dispose();
            Client.Dispose();
        }
    }
}
