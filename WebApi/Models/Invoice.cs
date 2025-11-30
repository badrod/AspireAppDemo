namespace WebApi.Models
{
    public class Invoice
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Vendor { get; set; } = string.Empty;
        public string Customer { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public List<InvoiceItem> Items { get; set; } = new();

        // Optional: raw AI JSON response for debugging
        public string? AIJsonResponse { get; set; }
        public List<InvoiceFile>? Files { get; set; }
    }

    public class InvoiceItem
    {
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => Quantity * UnitPrice;
    }
}
