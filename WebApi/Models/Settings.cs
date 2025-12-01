using Microsoft.FeatureManagement;

namespace WebApi.Models
{
    public class Settings
    {
       
        public OpenAISettings? OpenAISettings { get; set; } = new();

        public AzureOpenAISettings? AzureOpenAISettings { get; set; } = new();
    }
    public class AzureSettings
    {
        public string StorageAccount { get; set; } = string.Empty;
        public string KeyVaultUrl { get; set; } = string.Empty;
    }

    public class AzureOpenAISettings
    {
        public string? Endpoint { get; set; }
        public string? DeploymentName { get; set; }

    }

    public class OpenAISettings
    {
        public string? Model { get; set; }
        public string? ApiKey { get; set; }
    }
}
    