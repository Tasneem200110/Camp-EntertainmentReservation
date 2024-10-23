using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace BLL.Interfaces
{

    public interface IPhotoUploadService
    {
        Task<string> AddPhoto(IFormFile file, bool userDefaultFlag);
        Task<List<string>> AddPhotos(List<IFormFile> files, bool userDefaultFlag);
        Task<DeletionResult> DeletePhoto(string publicId);
    }
}