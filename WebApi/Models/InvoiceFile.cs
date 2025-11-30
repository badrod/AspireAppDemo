using System.Security.Cryptography;

namespace WebApi.Models
{
    public class InvoiceFile
    {
        public string? ContentType { get; set; }
        public byte[]? Content { get; set; }

        // Generate a safe file name with timestamp + original extension
        public string FileName => $"invoice-{DateTime.UtcNow:yyyyMMddHHmmss}{Path.GetExtension(OriginalFileName)}";

        // Original filename from upload, for safety we do not use it as the saved file name
        public string? OriginalFileName { get; set; }

        required public string Sha256 { get; set; }

        // Convenience property to get just the extension (e.g. ".pdf", ".jpg")
        public string? FileExtension => Path.GetExtension(OriginalFileName);


         

    }
}