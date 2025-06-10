using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rawy.APIs.Dtos.CommentDto;
using Rawy.APIs.Dtos.StoryDtos;
using Rawy.APIs.Services.CommentService;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Models;
using Rawy.DAL.Models.StorySpec;
using System.Security.Claims;

namespace Rawy.APIs.Controllers
{

    public class CommentController : BaseApiController
	{
		private readonly ICommentService _commentService;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CommentController(ICommentService commentService, IHttpContextAccessor httpContextAccessor)
		{
			_commentService = commentService;
			_httpContextAccessor = httpContextAccessor;
		}


		//  Add Comment to Story
		[HttpPost]
		public async Task<IActionResult> AddComment([FromBody] AddCommentDto dto,int userId)
		{
			//var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

			//if (userIdClaim == null)
			//	return Unauthorized("User is not authenticated");

			//var userId = int.Parse(userIdClaim.Value);
			var result = await _commentService.AddCommentAsync(dto, userId);

			return Ok(result);
			//return Ok("Comment added successfully.");
		}

		//  Get All Comments for a Story
		[HttpGet("{storyId}")]
		public async Task<IActionResult> GetComments(int storyId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var result = await _commentService.GetCommentsByStoryIdAsync(storyId);
			return Ok(result);

		}

		[HttpPut]
		public async Task<IActionResult> EditComment([FromBody] CommentUpdateDto commentDto)
		{
			var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

			if (userIdClaim == null)
				return Unauthorized("User is not authenticated");

			var userId = int.Parse(userIdClaim.Value);
			var result = await _commentService.UpdateCommentAsync(commentDto, userId);
			return Ok(result);
		}


		// Delete Comment
		[HttpDelete("{commentId}")]
		public async Task<IActionResult> DeleteComment(int commentId)
		{
			var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

			if (userIdClaim == null)
				return Unauthorized("User is not authenticated");

			var userId = int.Parse(userIdClaim.Value);
			var result = await _commentService.DeleteCommentAsync(commentId, userId);
			return Ok(result);
		}
	}
}

