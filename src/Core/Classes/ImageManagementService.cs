using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Core.Classes
{
    public class ImageManagementService : IImageManager
    {
        private readonly IImageRepository _repository;

        public ImageManagementService(IImageRepository repository)
        {
            _repository = repository;
        }

        public void ClearAllImages()
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");
            var imageDirectoryInfo = new DirectoryInfo(imagePath);

            foreach (var image in imageDirectoryInfo.GetFiles())
            {
                image.Delete();
            }
        }

        public async Task DeleteImage(Image image){
            await _repository.DeleteImage(image);
            DeleteFromDiks(image);
        }

        public List<Image> ListImages() => _repository.ListImages();

        private void DeleteFromDiks(Image image){
            try
            {
                var imageExtension = ".png";
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");

                var completeImagePath = Path.Combine(imagePath, string.Concat(image.Name, imageExtension));

                File.Delete(completeImagePath);
            }
            catch (System.Exception)
            {                
                throw;
            }
        }
    }
}