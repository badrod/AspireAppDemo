using WebApi.Models;
using WebApi.Services;


namespace Tests.Unit.Services
{

 
    public class OpenAIServiceTests 
    {
       
        private readonly FakeOpenAIService _service;
        public OpenAIServiceTests() { 
        
            _service = new  FakeOpenAIService();          
        }

        [Fact]
        public async Task ReadInvoiceAsync_PdfFile_ReturnsMockedInvoice()
        {
                

            var invoiceFile = new InvoiceFile
            {
                OriginalFileName = "test.pdf",
                ContentType = "application/pdf",
                Content = new byte[] { 1, 2, 3 },
                Sha256 = "abc123"
            };
         var result = await _service.ExtractInvoice(invoiceFile);

            Assert.Equal("INV-2025-001", result.InvoiceNumber);
            Assert.Equal(1250.75m, result.Total);
            Assert.Equal("test.pdf", result.Files?[0].OriginalFileName);
        }

        [Fact]
        public async Task ReadInvoiceAsync_ImageFile_ReturnsMockedInvoice()
        {
      

            var invoiceFile = new InvoiceFile
            {
                OriginalFileName = "invoice.png",
                ContentType = "image/png",
                Content = new byte[] { 1, 2, 3 },
                Sha256 = "def456"
            };
            var result = await _service.ExtractInvoice(invoiceFile);


            Assert.Equal("INV-2025-001", result.InvoiceNumber);
            Assert.Equal("Demo Supplies AB", result.Vendor);
            Assert.Equal("invoice.png", result.Files?[0].OriginalFileName);
        }

  
    }

}
