using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rawy.APIs.Dtos.StoryDtos;
using Rawy.APIs.Dtos;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Models.StorySpec;
using Rawy.DAL.Models;
using System.Security.Claims;

namespace Rawy.APIs.Controllers
{
	public class RatingController : BaseApiController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public RatingController(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
		}

		//  Add rate to Story

		[HttpPost("rate")]
		public async Task<IActionResult> AddOrUpdateRate([FromBody] RatingDto ratingDto)
		{
			try
			{
				if (ratingDto == null)
					return BadRequest("Invalid rating data.");

				if (ratingDto.Score < 0 || ratingDto.Score > 5)
					return BadRequest("Rating value must be between 0 and 5.");

				// Get user ID from token
				var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
				if (userIdClaim == null)
					return Unauthorized("User is not authenticated");

				var userId = int.Parse(userIdClaim.Value);

				// Check if rating already exists for this user and story
				var existingRating = await _unitOfWork.RatingRepository
					.GetRatingByUserAndStoryAsync(userId, ratingDto.StoryId); 

				if (existingRating != null)
				{
					// Update existing rating
					existingRating.Score = ratingDto.Score;
					existingRating.CreatedAt = DateTime.UtcNow;
					_unitOfWork.RatingRepository.UpdateAsync(existingRating);
					_unitOfWork.Complete();

					return Ok("Rating updated successfully.");
				}
				else
				{
					// Add new rating
					var rating = _mapper.Map<Rating>(ratingDto);
					rating.AppUserId = userId;
					rating.CreatedAt = DateTime.UtcNow;

					
					await _unitOfWork.RatingRepository.AddRatingAsync(rating);
					

					return Ok("Rating added successfully.");
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
			}
		}
		//[HttpPost]
		//public async Task<IActionResult> AddRate([FromBody] RatingDto RatingtDto)
		//{
		//	try
		//	{
		//		if (RatingtDto == null) return BadRequest("Invalid Rating data.");

		//		// Check rating range (assuming property is called Value)
		//		if (RatingtDto.Score < 0 || RatingtDto.Score > 5)
		//			return BadRequest("Rating value must be between 0 and 5.");

		//		// Extarct UserId From Token
		//		var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

		//		if (userIdClaim == null)
		//			return Unauthorized("User is not authenticated");

		//		var userId = int.Parse(userIdClaim.Value);

		//		var rating = _mapper.Map<Rating>(RatingtDto);
		//		rating.AppUserId = userId;


		//		await _unitOfWork.RatingRepository.AddRatingAsync(rating);

		//		var ratingResponse = new AddRateRespDto
		//		{
		//			Id = rating.Id,
		//			Score = rating.Score,
		//			//WriterName = rating.AppUser.FirstName + " " + rating.AppUser.LastName,
		//		//	StoryTitle = rating.Story.Title
		//		};



		//		return Ok(ratingResponse);
		//	}
		//	catch (InvalidOperationException ex)
		//	{
		//		return BadRequest(new { message = ex.Message }); // Handle duplicate rating
		//	}
		//	catch (Exception ex)
		//	{
		//		return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
		//	}

		//}

		//  Get All Rating for a Story
		[HttpGet("{storyId}")]
		public async Task<IActionResult> GetRating(int storyId)
		{

			var spec = new RatingOfStorySpec(storyId);

			var ratings = await _unitOfWork.RatingRepository.GetRatingByStoryIdAsync(spec);

			var ratingDtos = ratings.Select(c => new RatingResponseDto
			{
				Id = c.Id,
				Score = c.Score,
				WriterName = $"{c.AppUser.FirstName} {c.AppUser.LastName}", // Avoid circular reference
				StoryTitle = c.Story.Title // Avoid circular reference
			}).ToList();

			return Ok(ratingDtos);

		}

		[HttpGet("is-rated/{storyId}")]
		public async Task<IActionResult> HasUserRated(int storyId)
		{
			try
			{
				// Get user ID from token
				var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

				if (userIdClaim == null)
					return Unauthorized("User is not authenticated");

				var userId = int.Parse(userIdClaim.Value);

				// Check if rating exists
				var hasRated = await _unitOfWork.RatingRepository.HasUserRatedAsync(userId, storyId);

				return Ok(new { hasRated });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
			}
		}

		//// Get AverageRating
		//[HttpGet("average-rating/{storyId}")]
		//public async Task<IActionResult> GetAverageRating(int storyId)
		//{
		//	var story = await _unitOfWork.StoryRepository.GetByIdAsync(storyId);
		//	if (story == null)
		//	{
		//		return NotFound(new ApiResponse(404));
		//	}

		//	var averageScore = await _unitOfWork.RatingRepository.GetAverageRatingByStoryIdAsync(storyId);
		//	return Ok(new
		//	{
		//		StoryTitle = story.Title,
		//		AverageRating = averageScore
		//	});
		//}

		// Delete Rating
		[HttpDelete("{ratingId}")]
		public async Task<IActionResult> DeleteRating(int ratingId)
		{
			var rating = await _unitOfWork.RatingRepository.GetByIdAsync(ratingId);
			if (rating == null)
			{
				return NotFound("rating not found.");
			}

			await _unitOfWork.RatingRepository.DeleteAsync(ratingId);
			var deleted = _unitOfWork.Complete();


			return Ok(new { message = "Rating deleted successfully." });
		}
	}
}
