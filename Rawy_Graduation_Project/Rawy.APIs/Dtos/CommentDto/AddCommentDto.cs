namespace Rawy.APIs.Dtos.CommentDto
{
    public class AddCommentDto
    {
        public int StoryId { get; set; }  // Stoey being commente
        public int AppUserId { get; set; } // Writer who write comment
        public string Content { get; set; }  // Actual Comment
    }
}
