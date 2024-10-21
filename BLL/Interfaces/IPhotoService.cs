
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace BLL.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhoto(IFormFile file);
        Task<DeletionResult> DeletePhoto(string publicId);
    }
}
