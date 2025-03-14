using Rawy.DAL.Models;

namespace Rawy.APIs.Dtos.StoryDtos
{
	public class StoryResponseDto
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string Category { get; set; }
		public DateTime CreatedAt { get; set; }

		// Writer Details
		public string WriterId { get; set; }
		public string WriterName { get; set; }

		//  Story Average Rating
		public double? AverageRating { get; set; }
	}
}
