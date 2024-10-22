using BLL.Helpers;
using BLL.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BLL.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IConfiguration _configuration;

        public PhotoService(IConfiguration configuration, IOptions<CloudinarySetting> config)
        {
            _configuration = configuration;
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }


        public async Task<string> AddPhoto(IFormFile file, bool userFlag)
        {
            string resultURL;
            if (file == null || file.Length == 0) return resultURL = GetDefaultImage(userFlag);

            var uploadResult = new ImageUploadResult();
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
            resultURL = uploadResult.Url.ToString();

            return resultURL;
        }

        public async Task<DeletionResult> DeletePhoto(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result;
        }

        public string GetDefaultImage(bool userFlag)
        {
            if (userFlag)
                return _configuration["DefaultImages:User"];
            return _configuration["DefaultImages:Camp"];
        }
    }
}