using System.IO;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IImageDownloader
    {
        Task<byte[]> Download(Image image);        
    }
}