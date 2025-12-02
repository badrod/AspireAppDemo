using Microsoft.Extensions.Options;
using OpenAI.Files;
using OpenAI.Responses;
using System.ClientModel;
using WebApi.Models;

namespace WebApi.Services
{
#pragma warning disable OPENAI001

    public class OpenAIService(IOptions<Settings> options, ILogger<OpenAIService> logger) : IAIService
    {
        private readonly OpenAISettings settings = options.Value.OpenAISettings
                ?? throw new InvalidOperationException("OpenAI settings not configured.");

        private readonly string prompt = """
            Extract invoice fields (number, date, total, line items) and return as JSON.
            """;

        private readonly ResponseCreationOptions responseType = new()
        {
            TextOptions = new ResponseTextOptions
            {
                TextFormat = ResponseTextFormat.CreateJsonObjectFormat()
            }
        };
        public async Task<Invoice> ExtractInvoice(InvoiceFile invoiceFile)
        {
            try
            {
                var extension = Path.GetExtension(invoiceFile.OriginalFileName)?.ToLowerInvariant();

                var res = extension switch
                {
                    ".pdf" => await ReadInvoiceFromPdfAsync(invoiceFile),
                    ".png" or ".jpg" or ".jpeg" => await ReadInvoiceFromImageAsync(invoiceFile),
                    _ => throw new NotSupportedException($"File type {extension} is not supported.")
                };
                return MapResult(res);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to Read Invoice");
                throw;
            }

        }

        //If the invoiceFile is a PDF, we use a new file upload approach with GPT-5.1
        private async Task<ClientResult<OpenAIResponse>> ReadInvoiceFromPdfAsync(InvoiceFile invoiceFile)
        {
            var client = new OpenAIResponseClient("gpt-5.1", settings.ApiKey);
            var fileClient = new OpenAIFileClient(settings.ApiKey);
            OpenAIFile file;

            using (var stream = new MemoryStream(invoiceFile.Content!))
            {
                file = await fileClient.UploadFileAsync(stream, invoiceFile.FileName, FileUploadPurpose.UserData);
            }

            return await client.CreateResponseAsync(
                [
                 ResponseItem.CreateUserMessageItem(
                    [
                        ResponseContentPart.CreateInputFilePart(file.Id),
                        ResponseContentPart.CreateInputTextPart(prompt)
                    ])
                ], responseType
                );

        }

        //If the invoiceFile is an image, we use direct image input with GPT-4.1
        private async Task<ClientResult<OpenAIResponse>> ReadInvoiceFromImageAsync(InvoiceFile invoiceFile)
        {
            var client = new OpenAIResponseClient("gpt-4.1", settings.ApiKey);
            return await
                client.CreateResponseAsync(
                    [
                        ResponseItem.CreateUserMessageItem(
                        [
                            ResponseContentPart.CreateInputImagePart(BinaryData.FromBytes(invoiceFile.Content!),invoiceFile.ContentType!),
                            ResponseContentPart.CreateInputTextPart(prompt)
                        ])
                    ], responseType

                   );

        }


        public static Invoice MapResult( ClientResult<OpenAIResponse>? response)
        {
            var json = response?.Value?.GetOutputText();

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new InvalidOperationException("AI response contained no output text.");
            }

            return Invoice.FromJson(json);


        }
#pragma warning restore OPENAI001


    }
}