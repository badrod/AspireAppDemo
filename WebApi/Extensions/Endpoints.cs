using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
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



            ////use feature toggle 
            app.Use(async (context, next) =>
            {
                var toggles = context.RequestServices
                    .GetRequiredService<IOptionsSnapshot<Settings>>()
                    .Value.FeatureToggles;

                if (!toggles.UseScalar && context.Request.Path.StartsWithSegments("/api"))
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    return;
                }

                await next();
            });


            app.MapGet("/features", (IOptionsSnapshot<Settings> options) =>
            {
                var toggles = options.Value.FeatureToggles;
                return Results.Ok(new
                {
                    toggles.UseScalar,
                    toggles.UseCache,
                    toggles.EnableNewUI
                });
            }).WithDescription("Feature toggle");

        }

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