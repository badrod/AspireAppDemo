namespace WebApi.Models
{
    public class Settings
    {
        public bool UseAzureOpenAI  { get; set; }
        public OpenAISettings? OpenAISettings { get; set; }

        public AzureOpenAISettings? AzureOpenAISettings { get; set; }
    }

    public class AzureOpenAISettings
    {
        public string? Endpoint { get; set; }
        public string? DeploymentName { get; set; }
        public string? ApiKey { get; set; }
    }

    public class OpenAISettings
    {
        public string? Endpoint { get; set; }
        public string? DeploymentName { get; set; }
        public string? ApiKey { get; set; }
    }
}
    