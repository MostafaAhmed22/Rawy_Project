using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using Rawy.DAL.Models;

namespace Rawy.APIs.Services.Photo
{
	public class PhotoService : IPhotoService
	{
		private readonly Cloudinary _cloudinary;

		public PhotoService(IOptions<CloudinarySettings> config)
		{
			var acc = new Account("dvw90hqi7", "287947226172854", "SSIrDfZzG1jjeaAPl0DbbBJvRm0");//new Account(

			//	config.Value.CloudName,
			//	config.Value.ApiKey,
			//	config.Value.ApiSecret
			//);
			_cloudinary = new Cloudinary(acc);
		}

		public async Task<ImageUploadResult> UploadPhotoAsync(IFormFile file)
		{
			var uploadResult = new ImageUploadResult();

			if (file.Length > 0)
			{
				using var stream = file.OpenReadStream();
				var uploadParams = new ImageUploadParams
				{
					File = new FileDescription(file.FileName, stream),
					Transformation = new Transformation().Height(500).Width(500).Crop("fill"),
					PublicId = Guid.NewGuid().ToString()
				};
				uploadResult = await _cloudinary.UploadAsync(uploadParams);
			}

			if (uploadResult.Error != null)
			{
				throw new Exception($"Cloudinary upload failed: {uploadResult.Error.Message}");
			}

			return uploadResult;
		}

		public async Task<DeletionResult> DeletePhotoAsync(string publicId)
		{
			var deleteParams = new DeletionParams(publicId);
			var result = await _cloudinary.DestroyAsync(deleteParams);
			return result;
		}
	}
}
