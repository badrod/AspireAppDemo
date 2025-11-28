using System.Security.Cryptography;

namespace WebApi.Extensions
{
    public static class Endpoints
    {
        public static void MapUpload(this WebApplication app)
        {
            // Simple upload endpoint for image or PDF
            app.MapPost("/api/upload", static async (HttpRequest request) =>
            {
                if (!request.HasFormContentType)
                {
                    return Results.BadRequest(new { error = "Content-type must be multipart/form-data" });
                }

                var form = await request.ReadFormAsync();
                var file = form.Files.FirstOrDefault();
                if (file is null)
                {
                    return Results.BadRequest(new { error = "No file provided" });
                }

                if (!file.ContentType.StartsWith("image/") && file.ContentType != "application/pdf")
                {
                    return Results.BadRequest(new { error = "Only images and PDFs are allowed" });
                }

                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var bytes = ms.ToArray();

                string sha256;
                sha256 = Convert.ToHexStringLower(SHA256.HashData(bytes));

                var result = new
                {
                    fileName = file.FileName,
                    contentType = file.ContentType,
                    length = bytes.Length,
                    sha256
                };

                return Results.Json(result);
            });
        

        }
    }
}
