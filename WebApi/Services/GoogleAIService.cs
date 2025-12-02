using Google.GenAI.Types;
using Microsoft.Extensions.Options;
using WebApi.Models;

namespace WebApi.Services
{

    public class GoogleAIService(IOptions<Settings> options, ILogger<GoogleAIService> logger) : IAIService
    {
        private readonly GoogleAISettings settings = options.Value.GoogleAISettings
            ?? throw new InvalidOperationException("Google AI settings not configured.");


        public async Task<Invoice> ExtractInvoice(InvoiceFile invoiceFile)
        {

            try
            {
                var json = await ExtractInvoiceFromFile(invoiceFile);

                var parsedInvoice = JsonHelper.Deserialize<Invoice?>(json!);
          

                return parsedInvoice ?? new Invoice { AIJsonResponse = json };


            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to Read Invoice");
                throw;
            }
        }



        private async Task<string?> ExtractInvoiceFromFile(InvoiceFile invoiceFile)
        {
            var client = new Google.GenAI.Client(apiKey: settings.ApiKey);

            var imagePart = new Part
            {
                InlineData = new Blob
                {
                    MimeType = invoiceFile.ContentType,
                    Data = invoiceFile.Content // must be byte[]
                }
            };

            var textPart = new Part { Text = "Extract invoice details from this file and respond ONLY with valid JSON that matches the privided invoice schema." };
            var content = new Content
            {
                Parts = new List<Part> { imagePart, textPart }
            };

            var outputConfig = new GenerateContentConfig
            {
                ResponseMimeType = "application/json",
                ResponseJsonSchema = Schema.FromJson(JsonHelper.GetSchema<Invoice>(), JsonHelper.Options),
                ResponseSchema = Schema.FromJson(JsonHelper.GetSchema<Invoice>(), JsonHelper.Options)

            };

            var response = await client.Models.GenerateContentAsync(
                model: "gemini-2.5-flash", // supports multimodal
                contents: content,
                outputConfig
                );


            // Defensive check
            var candidate = response?.Candidates?.FirstOrDefault();
            var textResult = candidate?.Content?.Parts?.FirstOrDefault(p => p.Text != null)?.Text;

            Console.WriteLine(textResult);
            if (string.IsNullOrWhiteSpace(textResult))
            {
                throw new InvalidOperationException("No text response from Gemini model.");
            }

            return textResult;
        }

    }
}