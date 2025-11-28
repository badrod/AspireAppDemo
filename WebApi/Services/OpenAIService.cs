using WebApi.Models;

namespace WebApi.Services
{

    public interface IOpenAIService
    {
        public Task<Invoice> ReadInvoice(InvoiceFile invoiceFile);
    }


    public class OpenAIService : IOpenAIService
    {
        public async Task<Invoice> ReadInvoice(InvoiceFile invoiceFile)
        {

            return new Invoice();
        }
    }
}
