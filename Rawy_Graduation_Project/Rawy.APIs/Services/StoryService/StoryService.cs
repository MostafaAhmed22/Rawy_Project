using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Rawy.APIs.Dtos;
using Rawy.APIs.Dtos.StoryDtos;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Models;
using Rawy.DAL.Models.Hubs;
using Rawy.DAL.Models.StorySpec;
using Microsoft.EntityFrameworkCore;

namespace Rawy.APIs.Services.StoryService
{
	public class StoryService : IStoryService
	{ 
		//private readonly IGenericRepository<Story> _storyRepo;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IHubContext<PostHub> _hubContext;
	public StoryService(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<PostHub> hubContext)
	{
		//_storyRepo = StoryRepo;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_hubContext = hubContext;
	}
	
		public async Task<ApiResponse> AddStory(AddStoryDto _story)
		{
			if (_story == null)
				return new ApiResponse(400, "Story data is required.");

			//  Validate WriterId
			if (_story.AppUserId == 0)
				return new ApiResponse(400, "WriterId is required.");

			//  Ensure Title and Content Are Not Empty
			if (string.IsNullOrWhiteSpace(_story.Title))
				return new ApiResponse(400, "Story title cannot be empty.");

			if (string.IsNullOrWhiteSpace(_story.Content))
				return new ApiResponse(400, "Story content cannot be empty.");

			var story = _mapper.Map<Story>(_story);
			await _unitOfWork.StoryRepository.AddAsync(story);
			var added = _unitOfWork.Complete();


			// Broadcast the story to all connected clients without calling Get Endpoint => SignalR
			await _hubContext.Clients.All.SendAsync("ReceiveStory", story);

			return added > 0
				? new ApiResponse(200, "Story created successfully", new { StoryId = story.Id })
				: new ApiResponse(500, "Failed To Add Story.");
			
		}

		public async Task<ApiResponse> DeleteStory(int storyId)
		{
			var story = await _unitOfWork.StoryRepository.GetByIdAsync(storyId);
			if (story == null)
			{
				return new ApiResponse(404,"Not Found");
			}

			await _unitOfWork.StoryRepository.DeleteAsync(story.Id);
			var deleted = _unitOfWork.Complete();
			return deleted > 0
				? new ApiResponse(200, "Story deleted successfully.", new { StoryId = story.Id })
				: new ApiResponse(500, "Failed to delete story.");
		}

		public async Task<IEnumerable<StoryResponseDto>> GetAll([FromQuery] StorySpecParams specParams)
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

			return responseDtos;
		}

		public async Task<StoryResponseDto> GetById(int id)
		{
			var spec = new StoryWithReview(id);
			var story = await _unitOfWork.StoryRepository.GetByIdWithSpecAsync(spec);
			if (story == null)
				throw new KeyNotFoundException($"Story with ID {id} not found.");


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
			return responseDto;
		}

		public async Task<ApiResponse> UpdateStory(int storyId, UpdateStoryDto storyDto, int userId)
		{
			var story = await _unitOfWork.StoryRepository.GetByIdAsync(storyId);
			if (story == null || story.AppUserId != userId)
				return new ApiResponse(404, "Story not found or unauthorized");

			//  Ensure the Story Exists

			
			_mapper.Map(storyDto, story);
			await _unitOfWork.StoryRepository.UpdateAsync(story);
			var Updated = _unitOfWork.Complete();

			// Broadcast the story to all connected clients without calling Get Endpoint => SignalR
			await _hubContext.Clients.All.SendAsync("ReceiveStory", story);

			return Updated > 0
				? new ApiResponse(200, "Story Updated successfully.", new { StoryId = story.Id })
				: new ApiResponse(500, "Failed to update story.");
		}

		
	}
}
