using Docker.DotNet.Models;
using Shouldly;
using System.Net;

namespace ProductCatalog.Acceptance.Tests.Tests
{
    public class HealthEndpointTests : IClassFixture<ApplicationFactory>
    {
        private readonly HttpClient _client;

        public HealthEndpointTests(ApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetHealth_ShouldReturnHealthy()
        {
            //Arrange & Act
            var response = await _client.GetAsync("/health");

            //Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.ShouldContain("Healthy");
        }
    }
}