using Microsoft.AspNetCore.Mvc;
using Rawy.DAL.Models.StorySpec;
using Rawy.DAL.Models;
using Rawy.APIs.Dtos.StoryDtos;
using Rawy.APIs.Dtos;

namespace Rawy.APIs.Services.StoryService
{
	public interface IStoryService
	{
		Task<IEnumerable<StoryResponseDto>> GetAll([FromQuery] StorySpecParams specParams);
		Task<StoryResponseDto> GetById(int id);
		 Task<ApiResponse> AddStory(AddStoryDto _story);
		 Task<ApiResponse> UpdateStory(int storyId, UpdateStoryDto storyDto, int userId);
		 Task<ApiResponse> DeleteStory(int id);

	}
}
