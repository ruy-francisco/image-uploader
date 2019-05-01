using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IImageRepository
    {
         int AmountOfFiles { get; }
         Task<int> AddImage(Image image);
         Task DeleteImage(Image image);
         List<Image> ListImages();
    }
}