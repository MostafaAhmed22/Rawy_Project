using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rawy.APIs.Dtos;
using Rawy.APIs.Dtos.StoryDtos;
using Rawy.APIs.Services.Photo;
using Rawy.BLL;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Models;
using Rawy.DAL.Models.WriterSpec;
using System.Security.Claims;

namespace Rawy.APIs.Controllers
{
	public class WriterController : BaseApiController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<AppUser> _userManager;
		private readonly IPhotoService _photoService;

		public WriterController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager,IPhotoService photoService)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_photoService = photoService;
		}

		// Get Writer By ID
		[HttpGet("GetProfile/{id}")]
		public async Task<IActionResult> GetProfileById(int id)
		{

			var spec = new WriterWithStoriesSpec(id);
			var writer = await _unitOfWork.UserRepository.GetByIdWithSpecAsync(spec);

			if (writer == null)
				return NotFound(new ApiResponse(404));

			var dto = new WriterProfileDto
			{
				Email = writer.Email,
				FName = writer.FirstName,
				LName = writer.LastName,
				PhoneNumber = writer.PhoneNumber,
				PhotoUrl = writer.ProfilePictureUrl,
				PhotoPublicId = writer.ProfilePicturePublicId,
				FollowersCount = writer.Followers?.Count ?? 0,
				FollowingsCount = writer.Followings?.Count ?? 0,
				Stories = writer.Stories.Select(s => new StoryDto
				{
					Title = s.Title,
					Content = s.Content,
					CreatedAt = s.CreatedAt
				}).ToList()
			};

			return Ok(dto);
		}


		[HttpGet]
		public async Task<IActionResult> GetAllWriters()
		{
			var writers = await _unitOfWork.UserRepository.GetAllAsync();

			var Users = writers.Select(writer => new WriterDto
			{
				Id = writer.Id,
				Email = writer.Email,
				FirstName = writer.FirstName,
				LastName = writer.LastName,
				PhoneNumber = writer.PhoneNumber

			}).ToList();
			return Ok(Users);
		}

			[HttpDelete("{id}")]
		//[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> DeleteWriter(int id)
		{
			var writer = await _unitOfWork.UserRepository.GetByIdAsync(id);
			if (writer == null)
				return NotFound(new { message = "Writer not found" });

			_unitOfWork.UserRepository.DeleteAsync(writer.Id);


			return Ok(new { message = "Writer deleted successfully" });
		}

		[HttpPost("follow")]
		public async Task<IActionResult> FollowWriter(int followeeId)
		{
			//    if (followerId == followedId)
			//    {
			//        return BadRequest("You can't follow yourself.");
			//    }

			//    var follower = await _unitOfWork.WriterRepository.GetByIdAsync(followerId);
			//    var followed = await _unitOfWork.WriterRepository.GetByIdAsync(followedId);

			//    //var follower = await _context.Writers.FindAsync(followerId);
			//    //var followed = await _context.Writers.FindAsync(followedId);

			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			var appUser = await _userManager.FindByEmailAsync(userEmail);
			var follower = await _unitOfWork.UserRepository.GetByIdAsync(appUser.Id);

			var followee = await _unitOfWork.UserRepository.GetByIdAsync(followeeId);


			//    if (followee == null || followed == null)
			if (followee == null || follower.Id == followee.Id)
				return BadRequest("Invalid follow request");


			var existingFollow = await _unitOfWork.FollowRepository.FindAsync(
				f => f.FollowerId == follower.Id && f.FolloweeId == followee.Id);

			if (existingFollow != null)
			{
				return BadRequest("Already following.");
			}

			var follow = new WriterFollow
			{
				FollowerId = follower.Id,
				FolloweeId = followee.Id
			};


			await _unitOfWork.FollowRepository.AddAsync(follow);


			return Ok("Followed successfully");
		}

		[HttpPost("unfollow")]
		public async Task<IActionResult> UnfollowEmployee(int followeeId)
		{
			#region MyRegion
			//    if (followerId == followedId)
			//    {
			//        return BadRequest("You can't unfollow yourself.");
			//    }

			//    var follow = await _context.WriterFollows
			//        .FirstOrDefaultAsync(x => x.FollowerId == followerId && x.FollowedId == followedId);

			//    if (follow == null)
			//    {
			//        return NotFound("Follow relationship does not exist.");
			//    }

			//    _context.WriterFollows.Remove(follow);
			//    await _context.SaveChangesAsync();

			//    return Ok("Unfollowed successfully.");
			//} 
			#endregion

			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			var appUser = await _userManager.FindByEmailAsync(userEmail);
			var follower = await _unitOfWork.UserRepository.GetByIdAsync(appUser.Id);

			var followee = await _unitOfWork.UserRepository.GetByIdAsync(followeeId);


			var follow = await _unitOfWork.FollowRepository.FindAsync(
				f => f.FollowerId == follower.Id && f.FolloweeId == followeeId);

			if (follow == null)
				return NotFound("You are not following this user");

			await _unitOfWork.FollowRepository.Delete(follow);


			return Ok("Unfollowed successfully");

		}

		[HttpPost("upload-photo")]
		public async Task<IActionResult> UploadPhoto(int id,IFormFile file)
		{

			//var userEmail = User.FindFirstValue(ClaimTypes.Email);
			//var writer = await _userManager.FindByEmailAsync(userEmail);
			var writer = await _unitOfWork.UserRepository.GetByIdAsync(id);

			if (writer == null) return NotFound("Writer not found");

			//  1. Delete existing photo if it exists
			if (!string.IsNullOrEmpty(writer.ProfilePicturePublicId))
			{
					var deleteResult = await _photoService.DeletePhotoAsync(writer.ProfilePicturePublicId);
					if (deleteResult.Result != "ok")
						return BadRequest("Failed to delete existing photo.");
			}

			// 2. Upload new photo
			var uploadResult = await _photoService.UploadPhotoAsync(file);

			if (uploadResult.Error != null)
				return BadRequest(uploadResult.Error.Message);
			writer.ProfilePictureUrl = uploadResult.SecureUrl.ToString();
			writer.ProfilePicturePublicId = uploadResult.PublicId;
			_unitOfWork.UserRepository.UpdateAsync(writer);
			

			return Ok(new { photoUrl = writer.ProfilePictureUrl,PublicId = writer.ProfilePicturePublicId });
		}

		[HttpDelete("delete-photo")]
		public async Task<IActionResult> DeletePhoto(int id)
		{
			//var userEmail = User.FindFirstValue(ClaimTypes.Email);
			//var writer = await _userManager.FindByEmailAsync(userEmail);
			var writer = await _unitOfWork.UserRepository.GetByIdAsync(id);
			if (writer == null || string.IsNullOrEmpty(writer.ProfilePicturePublicId))
				return NotFound("No photo to delete");

			var result = await _photoService.DeletePhotoAsync(writer.ProfilePicturePublicId);
			if (result.Result != "ok") return BadRequest("Failed to delete photo");

			writer.ProfilePictureUrl = null;
			writer.ProfilePicturePublicId = null;
			_unitOfWork.UserRepository.UpdateAsync(writer);

			return Ok("Photo deleted successfully");
		}
	}
}
