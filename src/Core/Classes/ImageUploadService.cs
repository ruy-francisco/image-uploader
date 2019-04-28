using System;
using System.IO;
using System.Threading.Tasks;
using Core.Entities;
using Core.Extension;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Classes
{
    public class ImageUploadService : IImageUploader
    {
        private readonly IImageRepository _repository;

        public ImageUploadService(IImageRepository respository)
        {
            _repository = respository;
        }

        public async Task<string> UploadImage(IFormFile inputImage, string imageName, string imageDescription)
        {
            if (_repository.AmountOfFiles == 8)
            {
                throw new Exception("Número máximo (8) de upload alcançado.");
            }

            if (inputImage.IsPng())
            {                
                var imagePath = await SaveImageInDisk(inputImage, imageName);

                var image = new Image {
                    Name = imageName,
                    Description = imageDescription,
                    Path = imagePath
                };

                await _repository.AddImage(image);
                return "Upload feito com sucesso";
            }

            throw new Exception("Formato da imagem incorreto. \nImagem deve ter formato PNG");
        }

        public async Task<string> SaveImageInDisk(IFormFile inputImage, string imageName)
        {
            try
            {
                var imageExtension = string.Format(".{0}", inputImage.ContentType.Substring(6, 3));
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");

                using (var imageBits = new FileStream(imagePath, FileMode.Create))
                {
                    await inputImage.CopyToAsync(imageBits);
                }

                return imagePath;
            }
            catch (Exception e)
            {                
                return e.Message;
            }
        }
    }
}