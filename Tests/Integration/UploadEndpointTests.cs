namespace Tests.Integration
{
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using System.Net;
    using System.Net.Http.Headers;
    using Tests.Unit.Services;
    using WebApi.Services;

    public class UploadEndpointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public UploadEndpointTests(WebApplicationFactory<Program> factory)
        {
            // Override DI to inject fake service
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<IAIService, FakeOpenAIService>();
                });
            });
        }

        [Fact]
        public async Task UploadEndpoint_ReturnsOk_ForPdfFile()
        {
            // Arrange
            var client = _factory.CreateClient();

            var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(new byte[] { 1, 2, 3 });
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");
            content.Add(fileContent, "file", "invoice.pdf");

            // Act
            var response = await client.PostAsync("/api/upload", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("INV-2025-001", body); // predictable fake result
        }
    }
}



