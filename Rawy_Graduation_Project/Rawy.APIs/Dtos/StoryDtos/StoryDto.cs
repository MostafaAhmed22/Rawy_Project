namespace Rawy.APIs.Dtos.StoryDtos
{
	public class StoryDto
	{

		public int Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public DateTime CreatedAt { get; set; }
		public double? AverageRating { get; set; }

		public int CommentCount { get; set; } // num of comments
	}
}
