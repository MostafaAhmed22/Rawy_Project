using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rawy.APIs.Dtos;
using Rawy.APIs.Dtos.StoryDtos;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Models;
using Rawy.DAL.Models.StorySpec;

namespace Rawy.APIs.Controllers
{

	public class CommentController : BaseApiController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CommentController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		//  Add Comment to Story
		[HttpPost]
		public async Task<IActionResult> AddComment([FromBody] CommentDto commentDto)
		{
			if (commentDto == null) return BadRequest("Invalid comment data.");

			var comment = _mapper.Map<Comment>(commentDto);
			comment.CreatedAt = DateTime.Now;

			await _unitOfWork.CommentRepository.AddAsync(comment);

			return Ok("Comment added successfully.");
		}

		//  Get All Comments for a Story
		[HttpGet("{storyId}")]
		public async Task<IActionResult> GetComments(int storyId)
		{

			var spec = new CommentsOfStorySpec(storyId);

			var comments = await _unitOfWork.CommentRepository.GetCommentsByStoryIdAsync(spec);

			var commentDtos = comments.Select(c => new CommentResponseDto
			{
				Id = c.Id,
				Content = c.Content,
				WriterName = $"{c.Writer.FName} {c.Writer.LName}", // Avoid circular reference
				StoryTitle = c.Story.Title // Avoid circular reference
			}).ToList();

			return Ok(commentDtos);

		}


		// Delete Comment
		[HttpDelete("{commentId}")]
		public async Task<IActionResult> DeleteComment(int commentId)
		{
			var comment = await _unitOfWork.CommentRepository.GetByIdAsync(commentId);
			if (comment == null)
			{
				return NotFound("Comment not found.");
			}

			await _unitOfWork.CommentRepository.DeleteAsync(commentId);
			

			return Ok(new { message = "Comment deleted successfully." });
		}
	}
}

