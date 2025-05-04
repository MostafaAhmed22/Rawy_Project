using AutoMapper;
using Rawy.APIs.Dtos;
using Rawy.APIs.Dtos.CommentDto;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Models;
using Rawy.DAL.Models.StorySpec;

namespace Rawy.APIs.Services.CommentService
{
    public class CommentService : ICommentService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}


		public async Task<ApiResponse> AddCommentAsync(AddCommentDto commentDto, int UserId)
		{
			if (string.IsNullOrWhiteSpace(commentDto.Content))
				return new ApiResponse(400, "Comment content is required.");

			var story = await _unitOfWork.StoryRepository.GetByIdAsync(commentDto.StoryId);
			if (story == null)
				return new ApiResponse(404, "Story not found.");

			var user = await _unitOfWork.UserRepository.GetByIdAsync(UserId);
			if (user == null)
				return new ApiResponse(404, "User not found.");

			var comment = _mapper.Map<Comment>(commentDto);
			comment.AppUserId = UserId;
			comment.CreatedAt = DateTime.Now;

			await _unitOfWork.CommentRepository.AddAsync(comment);
			var added = _unitOfWork.Complete();
			return new ApiResponse(201, "Comment added successfully.", new {CommentId = comment.Id});
		}

		public async Task<ApiResponse> UpdateCommentAsync(CommentUpdateDto dto)
		{
			var comment = await _unitOfWork.CommentRepository.GetByIdAsync(dto.CommentId);
			if (comment == null)
				return new ApiResponse(404, "Comment not found.");

			if (comment.AppUserId != dto.UsertId)
				return new ApiResponse(401, "You are not authorized to update this comment.");

			if (string.IsNullOrWhiteSpace(dto.Content))
				return new ApiResponse(400, "Updated content cannot be empty.");

			comment.Content = dto.Content;
			comment.UpdatedAt = DateTime.Now;
			await _unitOfWork.CommentRepository.UpdateAsync(comment);
			var updated = _unitOfWork.Complete();
			return updated > 0
				? new ApiResponse(200, "Comment updated successfully.", new {CommentId = comment.Id , UpdatedAt = comment.UpdatedAt})
				: new ApiResponse(500, "Failed to update comment.");

		}

		public async Task<ApiResponse> DeleteCommentAsync(int commentId, int userId)
		{
			var comment = await _unitOfWork.CommentRepository.GetByIdAsync(commentId);
			if (comment == null )
				return new ApiResponse(404, "Comment not found.");
		if(comment.AppUserId != userId)
				return new ApiResponse(401, "You are not authorized to delete this comment.");

			 await _unitOfWork.CommentRepository.DeleteAsync(comment.Id);
			var deleted = _unitOfWork.Complete();
			return deleted > 0
				? new ApiResponse(200, "Comment deleted successfully.", new { CommentId = comment.Id })
				: new ApiResponse(500, "Failed to delete comment.");
		}

		public async Task<IEnumerable<CommentResponseDto>> GetCommentsByStoryIdAsync(int storyId)
		{
			var spec = new CommentsOfStorySpec(storyId);

			var comments = await _unitOfWork.CommentRepository.GetCommentsByStoryIdAsync(spec);

			var commentDtos = comments.Select(c => new CommentResponseDto
			{
				Id = c.Id,
				Content = c.Content,
				WriterName = $"{c.AppUser.FirstName} {c.AppUser.LastName}", // Avoid circular reference
				StoryTitle = c.Story.Title // Avoid circular reference
			}).ToList();
			return commentDtos;
		}
	}
}
