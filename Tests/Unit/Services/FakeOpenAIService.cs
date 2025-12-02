using WebApi.Models;
using WebApi.Services;

namespace Tests.Unit.Services
{
    public interface IAIClient
    {
        Task<string> ExtractInvoice(InvoiceFile file);
    }
    public class FakeOpenAIService : IAIService
    {

        public Task<Invoice> ExtractInvoice(InvoiceFile file)
        {
            return Task.FromResult(new Invoice
            {
                InvoiceNumber = "INV-2025-001",
                InvoiceDate = DateTime.Now.ToString(),
                BillToName = "Test Consulting Group",
                VendorName = "Demo Supplies AB",
                Items = new List<InvoiceItem> { new InvoiceItem { Description = "Laptop", Quantity = 2, UnitPrice = 500 },
                 new InvoiceItem { Description = "Mouse", Quantity =5, UnitPrice = 25.15m }},
                TotalAmount = 1250.75m,
                Files = new List<InvoiceFile> { file },
                AIJsonResponse = @"{
                ""InvoiceNumber"": ""INV-2025-001"",        
                ""Date"": ""2025-11-30"",
                ""Vendor"": ""Demo Supplies AB"",
                ""Customer"": ""Test Consulting Group"",
                ""Total"": 1250.75,
                ""Items"": [
                    { ""Description"": ""Laptop"", ""Quantity"": 2, ""UnitPrice"": 500.00 },
                    { ""Description"": ""Mouse"", ""Quantity"": 5, ""UnitPrice"": 25.15 }
                ]
           }"
            });

        }
    }

    public class FakeOpenAIClient : IAIClient
    {
        public Task<string> ExtractInvoice(InvoiceFile file)
        {
            // Return mocked JSON regardless of input
            return Task.FromResult(@"{
                ""InvoiceNumber"": ""INV-2025-001"",        
                ""Date"": ""2025-11-30"",
                ""Vendor"": ""Demo Supplies AB"",
                ""Customer"": ""Test Consulting Group"",
                ""Total"": 1250.75,
                ""Items"": [
                    { ""Description"": ""Laptop"", ""Quantity"": 2, ""UnitPrice"": 500.00 },
                    { ""Description"": ""Mouse"", ""Quantity"": 5, ""UnitPrice"": 25.15 }
                ]
            }");
        }
    }

}
