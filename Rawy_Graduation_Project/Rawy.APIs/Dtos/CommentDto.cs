namespace Rawy.APIs.Dtos
{
	public class CommentDto
	{
        public string StoryId { get; set; }  // Stoey being commented
		public string WriterId { get; set; } // Writer who write comment
		public string Content { get; set; }  // Actual Comment
	}
}
