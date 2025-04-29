using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rawy.APIs.Dtos;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Models;

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
		public async Task<IActionResult> GetWriterById(int id)
		{
			var writer = await _unitOfWork.WriterRepository.GetByIdAsync(id);
			if (writer == null)
				return NotFound(new ApiResponse(404));

			return Ok(writer);
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

        //[HttpPost("follow")]
        //public async Task<IActionResult> FollowEmployee(int followerId, int followedId)
        //{
        //    if (followerId == followedId)
        //    {
        //        return BadRequest("You can't follow yourself.");
        //    }
            
        //    var follower = await _unitOfWork.WriterRepository.GetByIdAsync(followerId);
        //    var followed = await _unitOfWork.WriterRepository.GetByIdAsync(followedId);

        //    //var follower = await _context.Writers.FindAsync(followerId);
        //    //var followed = await _context.Writers.FindAsync(followedId);

        //    if (follower == null || followed == null)
        //    {
        //        return NotFound("Employee not found.");
        //    }

        //    var existingFollow = await _context.WriterFollows
        //        .FirstOrDefaultAsync(x => x.FollowerId == followerId && x.FollowedId == followedId);

        //    if (existingFollow != null)
        //    {
        //        return BadRequest("Already following this employee.");
        //    }

        //    var follow = new WriterFollow
        //    {
        //        FollowerId = followerId,
        //        FollowedId = followedId
        //    };

        //    _context.EmployeeFollows.Add(follow);
        //    await _context.SaveChangesAsync();

        //    return Ok("Followed successfully.");
        //}

        //[HttpPost("unfollow")]
        //public async Task<IActionResult> UnfollowEmployee(int followerId, int followedId)
        //{
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

    }
}
