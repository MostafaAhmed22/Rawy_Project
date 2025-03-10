using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
		public async Task<ActionResult<Story>> GetById(string id)
		{
			var story = await _storyRepo.GetByIdAsync(id);
			if (story == null)
			{
				return NotFound(new ApiResponse(404));
			}
			return Ok(story);
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
			if (string.IsNullOrEmpty(_story.WriterId))
				return BadRequest("WriterId is required.");

			//  Ensure Title and Content Are Not Empty
			if (string.IsNullOrWhiteSpace(_story.Title))
				return BadRequest("Story title cannot be empty.");

			if (string.IsNullOrWhiteSpace(_story.Content))
				return BadRequest("Story content cannot be empty.");

			var story = _mapper.Map<Story>(_story);
			await _storyRepo.AddAsync(story);
			return Ok(story);
		}


		[HttpPut("{id}")]
		public async Task<ActionResult<Story>> UpdateStory(string id,UpdateStoryDto storyDto)
		{
			if (storyDto == null)
				return BadRequest("Story data is required.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			//  Ensure the Story Exists
			
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
		public async Task<ActionResult<Story>> DeleteStory(string id)
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
