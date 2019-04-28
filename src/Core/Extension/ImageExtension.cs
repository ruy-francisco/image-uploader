using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace Core.Extension
{
    public static class ImageExtension
    {
        public static bool IsPng(this IFormFile image)
        {
            byte[] imageBytes;

            using (var memoryStream = new MemoryStream())
            {
                image.CopyTo(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            var pngByteArray = new byte[] { 137, 80, 78, 71};
            return pngByteArray.SequenceEqual(imageBytes.Take(pngByteArray.Length));
        }
    }
}