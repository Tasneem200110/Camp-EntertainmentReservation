using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace BLL.Interfaces
{

    public interface IPhotoService
    {
        Task<string> AddPhoto(IFormFile file, bool userFlag);
        Task<DeletionResult> DeletePhoto(string publicId);
    }
}