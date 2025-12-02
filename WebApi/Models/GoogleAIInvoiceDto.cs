namespace WebApi.Models
{
    using System.Text.Json.Serialization;

    public class InvoiceDto
    {
        [JsonPropertyName("invoice_number")]
        public string? InvoiceNumber { get; set; }

        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("vendor_name")]
        public string? VendorName { get; set; }

        [JsonPropertyName("bill_to")]
        public BillToDto? BillTo { get; set; }

        [JsonPropertyName("line_items")]
        public List<LineItemDto>? LineItems { get; set; }

        [JsonPropertyName("total_amount")]
        public decimal? TotalAmount { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }
    }

    public class BillToDto
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }
    }

    public class LineItemDto
    {
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("amount")]
        public decimal? Amount { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }
    }

}
