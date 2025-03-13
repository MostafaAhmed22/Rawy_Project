namespace Rawy.APIs.Dtos.StoryDtos
{
    public class CommentDto
    {
		public string StoryId { get; set; } // The Story this comment belongs to
		public string WriterId { get; set; } // ID of the Writer who commented
		public string Content { get; set; } // The actual comment text
	}
}
