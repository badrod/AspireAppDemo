using System.Security.Cryptography;
using WebApi.Models;

namespace WebApi.Extensions
{
    public static class FileHelper
    {

        public static bool IsImage(string contentType)
        {
            return contentType.StartsWith("image/");
        }
        public static bool IsPdf(string contentType)
        {
            return contentType == "application/pdf";
        }
        public static bool IsSupportedFileType(string contentType)
        {
            return IsImage(contentType) || IsPdf(contentType);
        }

        public static List<InvoiceFile> CreateInvoiceFileFromFormFiles(this IFormFileCollection formFiles)
        { 
            return [.. formFiles.Select(f => f.CreateInvoiceFileFromFormFile())];
        }
        public static InvoiceFile CreateInvoiceFileFromFormFile(this IFormFile formFile)
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
