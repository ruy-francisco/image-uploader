using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IImageManager
    {
        Task DeleteImage(Image image);
        List<Image> ListImages();
        void ClearAllImages();
    }
}