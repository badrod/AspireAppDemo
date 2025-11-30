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


    }
}
