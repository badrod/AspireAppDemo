using System.Text.Json;
using WebApi.Extensions;

namespace WebApi.Models
{
    public class Invoice
    {
        public string? InvoiceNumber { get; set; }
        public string? InvoiceDate { get; set; }
        public string? VendorName { get; set; } 
        public string? BillToName { get; set; }
        public string? BillToAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public string? TotalCurrency { get; set; }
        public List<InvoiceItem>? Items { get; set; } = new();

        // Optional: raw AI JSON response for debugging
        public string? AIJsonResponse { get; set; }
        public List<InvoiceFile>? Files { get; set; }
        // Helper method to parse JSON into Invoice
     

    }

    public class InvoiceItem
    {
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Amount { get; set; }
        public string? Currency { get; set; }
    }


}
    
