namespace WebApi.Models
{
    public class Invoice
    {
        public DateTime InvoiceDate { get; set; }
        public int InvoiceId { get; set; }
        public string? CustomerName { get; set; }
        public string? Description { get; set; }
        public string? Phone { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }

        public decimal Total { get; set; }

        public List<InvoiceFile> Files { get; set; } = [];

        public List<InvoiceRow> Rows { get; set; } = [];
    }

    public class InvoiceRow
    {
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }




    }
}
