using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace Core.Extension
{
    public static class ImageExtension
    {
        public static bool IsPng(this IFormFile image) => image.ContentType.Equals("image/png");
    }
}