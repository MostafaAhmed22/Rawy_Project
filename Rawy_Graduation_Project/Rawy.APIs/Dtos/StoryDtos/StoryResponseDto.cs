using Rawy.DAL.Models;
using Rawy.APIs.Dtos.CommentDto;

namespace Rawy.APIs.Dtos.StoryDtos
{
	public class StoryResponseDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string Category { get; set; }
		public DateTime CreatedAt { get; set; }

		// Writer Details
		public int WriterId { get; set; }
		public string WriterName { get; set; }
		public string PhotoUrl { get; set; }
		public string PhotoPublicId { get; set; }

		//  Story Average Rating
		//public double? AverageRating { get; set; }
		public int LikestCount { get; set; } // num of LikestCount
	//	public int DisLikeCount { get; set; } // num of DisLikeCount 
		public int CommentCount { get; set; } // num of comments
    }
}
