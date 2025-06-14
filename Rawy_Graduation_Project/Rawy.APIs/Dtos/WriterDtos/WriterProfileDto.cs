using Rawy.APIs.Dtos.StoryDtos;

namespace Rawy.APIs.Dtos.WriterDtos
{
    public class WriterProfileDto
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? PhotoUrl { get; set; }
        public string? PhotoPublicId { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingsCount { get; set; }

        public List<StoryResponseDto> Stories { get; set; }
    }
}

