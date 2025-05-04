namespace Rawy.APIs.Dtos.CommentDto
{
	public class CommentDto
	{
		public int Id { get; set; }
		public string Content { get; set; }
		public string WriterUserName { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
