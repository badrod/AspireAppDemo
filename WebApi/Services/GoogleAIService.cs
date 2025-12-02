using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.Options;
using WebApi.Models;
using Type = Google.GenAI.Types.Type;

namespace WebApi.Services
{

    public class GoogleAIService(IOptions<Settings> options, ILogger<GoogleAIService> logger) : IAIService
    {
        private readonly GoogleAISettings settings = options.Value.GoogleAISettings
            ?? throw new InvalidOperationException("Google AI settings not configured.");

        private readonly string prompt = """
            Extract invoice fields (number, date, total, line items) and return as JSON.
            """;

        public async Task<Invoice> ExtractInvoice(InvoiceFile invoiceFile)
        {
           
            try
            {
                var extension = Path.GetExtension(invoiceFile.OriginalFileName)?.ToLowerInvariant();
                var json = await ExtractInvoiceFromFile(invoiceFile);
                if (json == null)
                    throw new Exception("No JSON extracted from invoice file.");
                return Invoice.FromJson(json);
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

            var textPart = new Part { Text = prompt ?? "Extract invoice details from this file." };
            var content = new Content
            {
                Parts = new List<Part> { imagePart, textPart }
            };

            var outputConfig = new GenerateContentConfig
            {
                ResponseSchema = new Schema
                {
                    Type = Type.OBJECT,
                    Properties = new Dictionary<string, Schema>
                    {
                        { "vendor", new Schema { Type = Type.STRING } },
                        { "date", new Schema { Type = Type.STRING } },
                        { "total", new Schema { Type = Type.NUMBER } }
                    }
                },
                ResponseMimeType = "application/json"
            };

            var response = await client.Models.GenerateContentAsync(
                model: "gemini-2.5-flash", // supports multimodal
                contents: content,
                outputConfig
                );


            // Defensive check
            var candidate = response?.Candidates?.FirstOrDefault();
            var textResult = candidate?.Content?.Parts?.FirstOrDefault(p => p.Text != null)?.Text;

            if (string.IsNullOrWhiteSpace(textResult))
            {
                throw new InvalidOperationException("No text response from Gemini model.");
            }

            return textResult;
        }

    }
}