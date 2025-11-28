namespace WebApi.Extensions
{
    public static class HttpRequestValidator
    {
        public static async Task<IResult> IsValidRequestAsync(this HttpRequest request)
        {
            if (!request.HasFormContentType)
            {
                return Results.BadRequest(new { error = "Content-type must be multipart/form-data" });
            }

            var form = await request.ReadFormAsync();
            if (form.Files == null || form.Files.Count < 1)
            {
                return Results.BadRequest(new { error = "No file provided" });
            }
            if (form.Files.Any(f => f.Length == 0))
            {
                return Results.BadRequest(new { error = "Empty file uploaded" });
            }
            if (form.Files.Any(f => string.IsNullOrEmpty(f.ContentType)))
            {
                return Results.BadRequest(new { error = "File content type is missing" });
            }
            if (form.Files.Any(f =>
                    !f.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase) &&
                    !f.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase)))
            {
                return Results.BadRequest(new { error = "Only images and PDFs are allowed" });
            }


            return Results.Ok();
        }



    }
}
