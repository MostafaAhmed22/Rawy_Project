using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Rawy.APIs.Dtos;
using Rawy.APIs.Dtos.StoryDtos;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Models;
using Rawy.DAL.Models.StorySpec;

namespace Rawy.APIs.Controllers
{
	
	public class StoryController : BaseApiController
	{
		//private readonly IGenericRepository<Story> _storyRepo;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public StoryController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			//_storyRepo = StoryRepo;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<Story>> GetAll([FromQuery] StorySpecParams specParams)
		{
			var spec = new StoryWithReview(specParams);
			var Stories = await _unitOfWork.StoryRepository.GetAllWithSpecAsync(spec);

			var responseDtos = Stories.Select(story => new StoryResponseDto
			{
				Id = story.Id,
				Title = story.Title,
				Content = story.Content,
				Category = story.Category,
				CreatedAt = story.CreatedAt,
				WriterId = story.WriterId,
				WriterName = $"{story.Writer.FName} {story.Writer.LName}",
				AverageRating = _unitOfWork.RatingRepository.GetAverageRatingByStoryIdAsync(story.Id).Result // Ensure async handling in a real case
				
			}).ToList();

			return Ok(responseDtos);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Story>> GetById(int id)
		{
			var spec = new StoryWithReview(id);
			var story = await _unitOfWork.StoryRepository.GetByIdWithSpecAsync(spec);
			if (story == null)
			{
				return NotFound(new ApiResponse(404));
			}

			var averageScore = await _unitOfWork.RatingRepository.GetAverageRatingByStoryIdAsync(id);

			var responseDto = new StoryResponseDto
			{
				Id = story.Id,
				Title = story.Title,
				Content = story.Content,
				Category = story.Category,
				CreatedAt = story.CreatedAt,
				WriterId = story.WriterId,
				WriterName = $"{story.Writer.FName} {story.Writer.LName}",
				AverageRating = averageScore
			};

			return Ok(responseDto);
		}

		[HttpPost]
		public async Task<ActionResult<Story>> AddStory(AddStoryDto _story)
		{
			// Validate Request Body
			if (_story == null)
				return BadRequest("Story data is required.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			//  Validate WriterId
			if (_story.WriterId == 0)
				return BadRequest("WriterId is required.");

			//  Ensure Title and Content Are Not Empty
			if (string.IsNullOrWhiteSpace(_story.Title))
				return BadRequest("Story title cannot be empty.");

			if (string.IsNullOrWhiteSpace(_story.Content))
				return BadRequest("Story content cannot be empty.");

			var story = _mapper.Map<Story>(_story);
			await _unitOfWork.StoryRepository.AddAsync(story);
			return Ok(story);
		}


		[HttpPut("{id}")]
		public async Task<ActionResult<Story>> UpdateStory(int id,UpdateStoryDto storyDto)
		{
			if (storyDto == null)
				return BadRequest("Story data is required.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			//  Ensure the Story Exists
			
			var story = await _unitOfWork.StoryRepository.GetByIdAsync(id);
			if (story == null)
			{
				return NotFound(new ApiResponse(404));
			}
			_mapper.Map(storyDto, story);
			await _unitOfWork.StoryRepository.UpdateAsync(story);
			return Ok(story);

		}

		[HttpDelete]
		public async Task<ActionResult<Story>> DeleteStory(int id)
		{
			var story = await _unitOfWork.StoryRepository.GetByIdAsync(id);
			if (story == null)
			{
				return NotFound(new ApiResponse(404));
			}
			
			await _unitOfWork.StoryRepository.DeleteAsync(story.Id);
			return Ok(story);

		}

	}
}
