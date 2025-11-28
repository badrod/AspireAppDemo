using WebApi.Models;

namespace WebApi.Services
{
    public interface IAIService
    {
        public Task<List<Invoice>> ReadInvoices(List<InvoiceFile> invoiceFiles);
    }
}
