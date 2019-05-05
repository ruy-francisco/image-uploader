using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CustomEntities = Core.Entities;
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
                var imagePath = await SaveImageInDisk(inputImage, imageName, imageDescription);
                return "Upload feito com sucesso";
            }

            throw new Exception("Formato da imagem incorreto. \nImagem deve ter formato PNG");
        }

        public async Task<string> SaveImageInDisk(IFormFile inputImage, string imageName, string imageDescription)
        {
            string completeImagePath = string.Empty;

            try
            {
                var imageExtension = string.Format(".{0}", inputImage.ContentType.Substring(6, 3));
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");

                completeImagePath = Path.Combine(imagePath, string.Concat(imageName, imageExtension));

                if (!File.Exists(completeImagePath))
                {
                    var image = new CustomEntities.Image {
                        Name = imageName,
                        Description = imageDescription,
                        Path = imagePath
                    };

                    await _repository.AddImage(image);
                }                 

                using (var fileStream = File.Create(completeImagePath))
                {
                    await inputImage.CopyToAsync(fileStream);
                    
                    var byteLength = fileStream.Length;
                    await fileStream.WriteAsync(new byte[] {}, 0, (int) byteLength);
                }

                return completeImagePath;
            }
            catch (Exception)
            {                
                return completeImagePath;
            }
        }
    }
}