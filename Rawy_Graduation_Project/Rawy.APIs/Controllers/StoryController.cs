using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Rawy.APIs.Dtos;
using Rawy.APIs.Dtos.StoryDtos;
using Rawy.APIs.Services.StoryService;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Models;
using Rawy.DAL.Models.Hubs;
using Rawy.DAL.Models.StorySpec;
using System.Security.Claims;

namespace Rawy.APIs.Controllers
{
	
	public class StoryController : BaseApiController
	{
		//private readonly IGenericRepository<Story> _storyRepo;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IHubContext<PostHub> _hubContext;
		public StoryController(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<PostHub> hubContext)
		{
			//_storyRepo = StoryRepo;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_hubContext = hubContext;
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
				Content = story.Content.Length > 200
							? story.Content.Substring(0, 200) + "..."
							: story.Content,
				Category = story.Category,
				CreatedAt = story.CreatedAt,
				WriterId = story.AppUserId,
				WriterName = $"{story.AppUser.FirstName} {story.AppUser.LastName}",
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
				WriterId = story.AppUserId,
				WriterName = $"{story.AppUser.FirstName} {story.AppUser.LastName}",
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
			if (_story.AppUserId == 0)
				return BadRequest("WriterId is required.");

			//  Ensure Title and Content Are Not Empty
			if (string.IsNullOrWhiteSpace(_story.Title))
				return BadRequest("Story title cannot be empty.");

			if (string.IsNullOrWhiteSpace(_story.Content))
				return BadRequest("Story content cannot be empty.");

			var story = _mapper.Map<Story>(_story);
			await _unitOfWork.StoryRepository.AddAsync(story);
			var added = _unitOfWork.Complete();


			// Broadcast the story to all connected clients without calling Get Endpoint => SignalR
			await _hubContext.Clients.All.SendAsync("ReceiveStory", story);


			var responseDto = new AddStoryResponseDto
			{
				Id = story.Id,
				Title = story.Title,
				Content = story.Content,
				Category = story.Category,
				CreatedAt = story.CreatedAt
			};
			return Ok(responseDto);
		}


		[HttpPut("{id}")]
		public async Task<ActionResult<Story>> UpdateStory(int id, UpdateStoryDto storyDto)
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
			story.UpdatedAt = DateTime.Now;
			await _unitOfWork.StoryRepository.UpdateAsync(story);
			var Updated = _unitOfWork.Complete();

			// Broadcast the story to all connected clients without calling Get Endpoint => SignalR
			await _hubContext.Clients.All.SendAsync("ReceiveStory", story);

			var responseDto = new UpdateStoryResonse
			{
				Id = story.Id,
				Title = story.Title,
				Content = story.Content,
				Category = story.Category,
				UpdatedAt = story.UpdatedAt

			};
			return Ok(responseDto);

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
			var deleted = _unitOfWork.Complete();
			return Ok(story);

		}




		#region StoryService
		//[HttpPost]
		//public async Task<ActionResult<Story>> AddStory(AddStoryDto _story)
		//{
		//	var story = _storyService.AddStory(_story);
		//	return Ok(story);
		//}


		//[HttpPut("{id}")]
		//public async Task<ActionResult<Story>> UpdateStory(int id, UpdateStoryDto storyDto, int userId)
		//{
		//	//var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
		//	var story = _storyService.UpdateStory(id, storyDto, userId);
		//	return Ok(story);

		//}

		//[HttpDelete]
		//public async Task<ActionResult<Story>> DeleteStory(int id)
		//{
		//	var story = _storyService.DeleteStory(id);
		//	return Ok(story);

		//}
		#endregion

	}
}
