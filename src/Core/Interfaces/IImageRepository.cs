using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IImageRepository
    {
         int AmountOfFiles { get; }
         Task<int> AddImage(Image image);
         Task<List<Image>> DeleteImage(Image image);
    }
}