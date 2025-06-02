namespace Rawy.APIs.Dtos.StoryDtos
{
	public class ReactToStoryDto
	{
		public int StoryId { get; set; }
		public bool IsLike { get; set; } // true for like, false for dislike
	}
}
