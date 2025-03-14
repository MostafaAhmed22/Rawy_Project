using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rawy.APIs.Dtos.StoryDtos;
using Rawy.APIs.Dtos;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Models.StorySpec;
using Rawy.DAL.Models;

namespace Rawy.APIs.Controllers
{

	public class RatingController : BaseApiController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public RatingController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		//  Add rate to Story
		[HttpPost]
		public async Task<IActionResult> AddRate([FromBody] RatingDto RatingtDto)
		{
			try
			{
				if (RatingtDto == null) return BadRequest("Invalid Rating data.");

				var rating = _mapper.Map<Rating>(RatingtDto);
				

				await _unitOfWork.RatingRepository.AddRatingAsync(rating);

				return Ok("Rating added successfully.");
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message }); // Handle duplicate rating
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
			}
			
		}

		//  Get All Rating for a Story
		[HttpGet("{storyId}/rating")]
		public async Task<IActionResult> GetRating(string storyId)
		{

			var spec = new RatingOfStorySpec(storyId);

			var ratings = await _unitOfWork.RatingRepository.GetRatingByStoryIdAsync(spec);

			var ratingDtos = ratings.Select(c => new RatingResponseDto
			{
				Id = c.Id,
				Score = c.Score,
				WriterName = $"{c.Writer.FName} {c.Writer.LName}", // Avoid circular reference
				StoryTitle = c.Story.Title // Avoid circular reference
			}).ToList();

			return Ok(ratingDtos);



		}


		// Get AverageRating
		[HttpGet("{storyId}/average-rating")]
		public async Task<IActionResult> GetAverageRating(string storyId)
		{
			var story = await _unitOfWork.StoryRepository.GetByIdAsync(storyId);
			if (story == null)
			{
				return NotFound(new ApiResponse(404));
			}

			var averageScore = await _unitOfWork.RatingRepository.GetAverageRatingByStoryIdAsync(storyId);
			return Ok(new
			{
				StoryTitle = story.Title,
				AverageRating = averageScore
			});
		}


		// Delete Rating
		[HttpDelete("{ratingId}")]
		public async Task<IActionResult> DeleteRating(string ratingId)
		{
			var rating = await _unitOfWork.RatingRepository.GetByIdAsync(ratingId);
			if (rating == null)
			{
				return NotFound("rating not found.");
			}

			await _unitOfWork.RatingRepository.DeleteAsync(ratingId);


			return Ok(new { message = "Rating deleted successfully." });
		}
	}
}
