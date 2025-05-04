using Rawy.APIs.Dtos;
using Rawy.APIs.Dtos.CommentDto;

namespace Rawy.APIs.Services.CommentService
{
    public interface ICommentService
	{
		//Task<ApiResponse> UpdateCommentAsync(CommentUpdateDto dto, int userId);
		Task<ApiResponse> AddCommentAsync(AddCommentDto dto, int userId);
		Task<ApiResponse> UpdateCommentAsync( CommentUpdateDto dto);
		Task<ApiResponse> DeleteCommentAsync(int commentId, int userId);
		Task<IEnumerable<CommentResponseDto>> GetCommentsByStoryIdAsync(int storyId);
	}
}
