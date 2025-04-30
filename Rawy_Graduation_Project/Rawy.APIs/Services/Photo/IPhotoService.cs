using CloudinaryDotNet.Actions;

namespace Rawy.APIs.Services.Photo
{
	public interface IPhotoService
	{
		Task<ImageUploadResult> UploadPhotoAsync(IFormFile file);
		Task<DeletionResult> DeletePhotoAsync(string publicId);


	}
}
