namespace Rawy.APIs.Dtos.StoryDtos
{
	public class AddStoryResponseDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string Category { get; set; }
		public DateTime CreatedAt { get; set; }

		// Writer Details

	}
}
