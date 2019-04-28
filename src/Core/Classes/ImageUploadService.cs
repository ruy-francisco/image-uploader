using System;
using System.IO;
using System.Threading.Tasks;
using Core.Extension;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Classes
{
    public class ImageUploadService : IImageUploader
    {
        public async Task<string> UploadImage(IFormFile image)
        {
           /*  if (AmountOfImages == 8) Implementar checagem de limite de upload
            {
                return "Upload de imagens ";
            } */

            if (image.IsPng())
            {
                return await SaveImageInDisk(image);
            }

            return "Formato da imagem incorreto. \nImagem deve ter formato PNG";
        }

        public async Task<string> SaveImageInDisk(IFormFile image)
        {
            try
            {
                var imageExtension = string.Format(
                    ".{0}", 
                    image.FileName.Split('.')[image.FileName.Split('.').Length - 1]);

                var imageName = Guid.NewGuid().ToString() + imageExtension;
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");

                using (var imageBits = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(imageBits);
                }

                return imageName;
            }
            catch (Exception e)
            {                
                return e.Message;
            }
        }
    }
}