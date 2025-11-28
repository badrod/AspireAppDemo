using System.Security.Cryptography;

namespace WebApi.Models
{
    public class InvoiceFile
    {
        public string? ContentType { get; set; }
        public byte[]? Content { get; set; }
        public string FileName { get => $"invoice-{DateTime.UtcNow}.json"; }

        // Original filename from upload, for safety we do not use it as the saved file name
        public string? OriginalFileName { get; set; }
        required public string Sha256 { get; set; }


    }
}
