using Microsoft.AspNetCore.Http.HttpResults;
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
       
            // Simple upload endpoint for image or PDF
            app.MapPost("/upload", UploadDocument)
                .WithDescription("Create Invoices with Vision AI")
                .Produces<List<Invoice>>(StatusCodes.Status201Created)
                .ProducesProblem(500)
                .ProducesProblem(400);


            //use feature toggle 
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

        private static async Task<IResult> UploadDocument(HttpRequest request, IAIService service)
        {

            var validation = await request.IsValidRequestAsync();
            if (validation is BadRequest<string>)
                return validation;

            var form = await request.ReadFormAsync();    


            var invoiceFiles = form.Files.CreateInvoiceFileFromFormFiles();
          
            var result = await service.ReadInvoices(invoiceFiles);

            return Results.Json(result);

        }

    }
}
