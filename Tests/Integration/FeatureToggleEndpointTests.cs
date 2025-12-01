using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Tests.Integration
{
    public class FeatureToggleEndpointTests(WebApplicationFactory<Program> factory)
                : IClassFixture<WebApplicationFactory<Program>> // Program = entry point of your API
    {
        private readonly WebApplicationFactory<Program> _factory = factory;


        // Test for a feature toggle endpoint test both on and off states of flag
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task FeatureToggle_ReturnsExpectedResponse(bool flagValue)
        {
            // Override config dynamically
            var factory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        { "FeatureManagement:TestFeature", flagValue.ToString() }
                    });
                });
            });

            var client = factory.CreateClient();

            var response = await client.GetAsync("/test");    

            Assert.Equal(response.IsSuccessStatusCode,flagValue);
        }
    }

  

 
}