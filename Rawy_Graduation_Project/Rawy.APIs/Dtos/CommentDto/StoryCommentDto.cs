namespace Rawy.APIs.Dtos.CommentDto
{
	public class StoryCommentDto
	{
		public int Id { get; set; }
		public string Content { get; set; }
		public int WriterId { get; set; }
		public string WriterName { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
