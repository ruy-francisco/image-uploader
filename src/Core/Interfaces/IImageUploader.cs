using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IImageUploader
    {
        Task<string> UploadImage(IFormFile image);
        Task<string> SaveImageInDisk(IFormFile image);
    }
}