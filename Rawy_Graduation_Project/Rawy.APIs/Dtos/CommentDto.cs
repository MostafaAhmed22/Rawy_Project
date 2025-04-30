namespace Rawy.APIs.Dtos
{
	public class CommentDto
	{
        public int StoryId { get; set; }  // Stoey being commented
		public int AppUserId { get; set; } // Writer who write comment
		public string Content { get; set; }  // Actual Comment
	}
}
