#pragma warning disable OPENAI001
using OpenAI.Responses;
using System.ClientModel;
using System.Text.Json;
using WebApi.Models;

namespace WebApi.Helpers
{
    public static class AIResponseMapper
    {
        public static Invoice MapResult(this ClientResult<OpenAIResponse>? response)
        {
            var json = response?.Value?.GetOutputText();

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new InvalidOperationException("AI response contained no output text.");
            }

            var deserialized = JsonSerializer.Deserialize<Invoice>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (deserialized is null)
            {
                throw new JsonException("Failed to deserialize AI response into Invoice.");
            }

            return deserialized;
        }
    }
}
    
#pragma warning restore OPENAI001
