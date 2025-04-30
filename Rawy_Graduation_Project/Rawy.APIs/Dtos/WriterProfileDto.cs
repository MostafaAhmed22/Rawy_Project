using Rawy.APIs.Dtos.StoryDtos;

namespace Rawy.APIs.Dtos
{
	public class WriterProfileDto
	{
		public string FName { get; set; }
		public string LName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string? PhotoUrl { get; set; }
		public string? PhotoPublicId { get; set; }
		public int FollowersCount { get; set; }
		public int FollowingsCount { get; set; }

		public List<StoryDto> Stories { get; set; }
	}
}

