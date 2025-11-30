using WebApi.Models;

namespace WebApi.Services
{
    public interface IAIService
    {
        public Task<Invoice> ExtractInvoice(InvoiceFile invoiceFile);
    }
}
