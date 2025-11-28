namespace WebApi.Models
{
    public class InvoiceFile
    {
        public string? ContentType { get; set; }
        public byte[]? Content { get; set; }
        public string? OriginalFileName { get; set; }
    }
}
