using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rawy.APIs.Dtos.StoryDtos;
using Rawy.APIs.Services.StoryLikeService;
using System.Security.Claims;

namespace Rawy.APIs.Controllers
{

	public class LikeConroller : BaseApiController
	{
		private readonly IStoryLikeService _storyLikeService;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public LikeConroller(IStoryLikeService storyLikeService, IHttpContextAccessor httpContextAccessor)
		{
			_storyLikeService = storyLikeService;
			_httpContextAccessor = httpContextAccessor;
		}

	

		/// <summary>
		/// React to a story (like or dislike).
		/// If same reaction is already set, it removes it (toggle off).
		/// If opposite reaction is set, it updates it.
		/// </summary>
		[HttpPost("react")]
		public async Task<IActionResult> ReactToStory([FromBody] ReactToStoryDto dto)
		{
			var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

			if (userIdClaim == null)
				return Unauthorized(new { message = "User is not authenticated" });

			var userId = int.Parse(userIdClaim.Value);
			

			await _storyLikeService.ReactToStoryAsync(dto.StoryId, userId, dto.IsLike);

			return Ok(new { message = "Reaction updated." });
		}

		/// <summary>
		/// Get user's current reaction on a specific story.
		/// Returns: like, dislike, none
		/// </summary>
		[HttpGet("reaction/{storyId}")]
		public async Task<IActionResult> GetReactionStatus(int storyId)
		{
			var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

			if (userIdClaim == null)
				return Unauthorized(new { message = "User is not authenticated" });

			var userId = int.Parse(userIdClaim.Value);

			var reaction = await _storyLikeService.GetReactionStatusAsync(storyId, userId);

			return Ok(new { storyId, reaction });
		}
	}
}
