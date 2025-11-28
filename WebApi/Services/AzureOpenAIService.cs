using Microsoft.Extensions.Options;
using WebApi.Models;

namespace WebApi.Services
{


    public class AzureOpenAIService(IOptions<Settings> options) : IAIService
    {
        public async Task<List<Invoice>> ReadInvoices(List<InvoiceFile> invoiceFiles)
        {
            var azureAISettings = options.Value.AzureOpenAISettings ?? throw new Exception("AzureOpenAISettings settings are not configured.");


            var invoice = new Invoice
            {
                Files = invoiceFiles
            };
            return [invoice];
        }
    }
}
