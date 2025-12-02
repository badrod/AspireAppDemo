using Scalar.AspNetCore;
using System.Security.Cryptography;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Extensions
{
    public static class Endpoints
    {
        public static void MapApiEndpoints(this WebApplication app)
        {
            app.MapPost("/api/upload", async (IFormFile file, IAIService service) =>
            {
                try
                {
                    var invoiceFile = CreateInvoiceFile(file);
                    var result = await service.ExtractInvoice(invoiceFile);
                    return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message, statusCode: 500);
                }
            }).DisableAntiforgery()//just in test
            .WithDescription("Create Invoices with Vision AI")
            .Accepts<IFormFile>("multipart/form-data")
            .Produces<List<Invoice>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }

        //
        private static InvoiceFile CreateInvoiceFile(IFormFile formFile)
        {
            using var ms = new MemoryStream();
            formFile.CopyTo(ms);
            var bytes = ms.ToArray();
            return new InvoiceFile
            {
                OriginalFileName = formFile.FileName,
                ContentType = formFile.ContentType,
                Content = bytes,
                Sha256 = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant()
            };

        }
    }
}