using System.Text.Json;
using WebApi.Models;

namespace WebApi.Models
{
    public class Invoice
    {
        public string? InvoiceNumber { get; set; } 
        public DateTime? Date { get; set; }
        public string? Vendor { get; set; } = string.Empty;
        public string? Customer { get; set; } = string.Empty;
        public decimal? Total { get; set; }
        public List<InvoiceItem>? Items { get; set; } = new();

        // Optional: raw AI JSON response for debugging
        public string? AIJsonResponse { get; set; }
        public List<InvoiceFile>? Files { get; set; }
        // Helper method to parse JSON into Invoice
        public static Invoice FromJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<Invoice>(json, new JsonSerializerOptions
                {
                    PropertyNamingPolicy=JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true,
                    Encoder= System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
                }) ?? throw new InvalidOperationException("Failed to parse invoice JSON.");
            }
            catch
            {

                return new Invoice { AIJsonResponse = json };
            }
        }

    }

    public class InvoiceItem
    {
        public string? Description { get; set; } 
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? LineTotal => Quantity * UnitPrice;
    }

public static class InvoiceMapper
{
    public static Invoice ToDomain(InvoiceDto dto)
    {
        return new Invoice
        {
            InvoiceNumber = dto.InvoiceNumber,
            Date = dto.Date,
            Vendor = dto.VendorName,
            Customer = dto.BillTo?.Name, // flattening BillTo.Name
            Total = dto.TotalAmount,
            Items = dto.LineItems?.Select(li => new InvoiceItem
            {
                Description = li.Description,
                Quantity = li.Quantity,
                UnitPrice = li.Amount
            }).ToList()
        };
    }
}
}