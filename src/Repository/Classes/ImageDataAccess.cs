using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Repository.DatabaseContexts;

namespace Repository.Classes
{
    public class ImageDataAccess : IImageRepository
    {
        public int AmountOfFiles { get => _imageContext.Images.Count(); }
        private readonly ImageContext _imageContext;

        public ImageDataAccess(ImageContext imageContext) => _imageContext = imageContext;

        public async Task<int> AddImage(Image image)
        {
            _imageContext.Add(image);
            return await _imageContext.SaveChangesAsync();
        }

        public async Task DeleteImage(Image image)
        {
            _imageContext.Remove(image);
            await _imageContext.SaveChangesAsync();
        }

        public List<Image> ListImages() => _imageContext.Images.ToList();
    }
}