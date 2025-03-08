using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using Rawy.APIs.Dtos;
using Rawy.APIs.Dtos.StoryDtos;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Models;

namespace Rawy.APIs.Controllers
{

	public class StoryController : BaseApiController
	{
		private readonly IGenericRepository<Story> _storyRepo;
		private readonly IMapper _mapper;

		public StoryController(IGenericRepository<Story> StoryRepo, IMapper mapper)
		{
			_storyRepo = StoryRepo;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<Story>> GetAll()
		{
			var Stories = await _storyRepo.GetAllAsync();

			return Ok(Stories);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Story>> GetById(int id)
		{
			var story = await _storyRepo.GetByIdAsync(id);
			if (story == null)
			{
				return NotFound(new ApiResponse(404));
			}
			return Ok(story);
		}

		[HttpPost]
		public async Task<ActionResult<Story>> AddStory(Story story)
		{
			//var story = _mapper.Map<Story>(story);
			await _storyRepo.AddAsync(story);
			return Ok(story);
		}
		//   public async Task<ActionResult<Story>> AddStory(AddStoryDto storyDto)
		//{
		//	var story = _mapper.Map<Story>(storyDto);
		//	await _storyRepo.AddAsync(story);
		//	return Ok(story);
		//   }


		[HttpPut("{id}")]
		public async Task<ActionResult<Story>> UpdateStory(int id,UpdateStoryDto storyDto)
		{
			var story = await _storyRepo.GetByIdAsync(id);
			if (story == null)
			{
				return NotFound(new ApiResponse(404));
			}
			_mapper.Map(storyDto, story);
			await _storyRepo.UpdateAsync(story);
			return Ok(story);

		}

		[HttpDelete]
		public async Task<ActionResult<Story>> DeleteStory(int id)
		{
			var story = await _storyRepo.GetByIdAsync(id);
			if (story == null)
			{
				return NotFound(new ApiResponse(404));
			}
			
			await _storyRepo.DeleteAsync(story.Id);
			return Ok(story);

		}

	}
}
