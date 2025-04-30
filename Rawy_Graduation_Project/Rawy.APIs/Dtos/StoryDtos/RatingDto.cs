namespace Rawy.APIs.Dtos.StoryDtos
{
	public class RatingDto
	{
		public int StoryId { get; set; } // The Story being rated
		public int AppUserId { get; set; } // ID of the Writer giving the rating
		public int Score { get; set; } // Rating score (1 to 5)
	}
}

