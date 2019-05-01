using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Classes
{
    public class ImageDownloadService : IImageDownloader
    {
        public async Task<byte[]> Download(Image image)
        {
            var imageExtension = ".png";
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");

            var completeImagePath = Path.Combine(imagePath, string.Concat(image.Name, imageExtension));

            using (var memoryStream = new MemoryStream())
            {
                using (var fileStream = File.OpenRead(completeImagePath))
                {
                    await fileStream.CopyToAsync(memoryStream);
                }

                memoryStream.Position = 0;
                return memoryStream.ToArray();
            }
        }
    }
}