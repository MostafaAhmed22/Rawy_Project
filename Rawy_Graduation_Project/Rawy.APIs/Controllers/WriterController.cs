using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rawy.APIs.Dtos;
using Rawy.APIs.Dtos.StoryDtos;
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

		public WriterController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
		}

		// Get Writer By ID
		[HttpGet("{id}")]
		public async Task<IActionResult> GetProfileById(int id)
		{
			//var writer = await _unitOfWork.WriterRepository.GetByIdAsync(id);
			//if (writer == null)
			//	return NotFound(new ApiResponse(404));


			var spec = new WriterWithStoriesSpec(id);
			var writer = await _unitOfWork.WriterRepository.GetByIdWithSpecAsync(spec);

			if (writer == null)
				return NotFound(new ApiResponse(404));

			var dto = new WriterProfileDto
			{
				Email = writer.AppUser.Email,
				FName = writer.FName,
				LName = writer.LName,
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
			var writers = await _unitOfWork.WriterRepository.GetAllAsync();
			return Ok(writers);
		}

		[HttpDelete("{id}")]
		//[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> DeleteWriter(int id)
		{
			var writer = await _unitOfWork.WriterRepository.GetByIdAsync(id);
			if (writer == null)
				return NotFound(new { message = "Writer not found" });

			_unitOfWork.WriterRepository.DeleteAsync(writer.WriterId);


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
			var follower = await _unitOfWork.WriterRepository.GetByIdAsync(appUser.Id);

			var followee = await _unitOfWork.WriterRepository.GetByIdAsync(followeeId);


			//    if (followee == null || followed == null)
			if (followee == null || follower.WriterId == followee.WriterId)
				return BadRequest("Invalid follow request");


			var existingFollow = await _unitOfWork.FollowRepository.FindAsync(
				f => f.FollowerId == follower.WriterId && f.FolloweeId == followee.WriterId);

			if (existingFollow != null)
			{
				return BadRequest("Already following.");
			}

			var follow = new WriterFollow
			{
				FollowerId = follower.WriterId,
				FolloweeId = followee.WriterId
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
			var follower = await _unitOfWork.WriterRepository.GetByIdAsync(appUser.Id);

			var followee = await _unitOfWork.WriterRepository.GetByIdAsync(followeeId);


			var follow = await _unitOfWork.FollowRepository.FindAsync(
				f => f.FollowerId == follower.WriterId && f.FolloweeId == followeeId);

			if (follow == null)
				return NotFound("You are not following this user");

			await _unitOfWork.FollowRepository.Delete(follow);


			return Ok("Unfollowed successfully");

		}
	}
}
