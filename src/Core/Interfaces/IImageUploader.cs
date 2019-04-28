using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IImageUploader
    {
        Task<string> UploadImage(IFormFile image, string imageName, string imageDescription);
    }
}