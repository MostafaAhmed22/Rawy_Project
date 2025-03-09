using Rawy.DAL.Models;

namespace Rawy.APIs.Dtos.StoryDtos
{
	public class AddStoryDto
	{
		public string Title { get; set; }
		public string Content { get; set; }
		public string Category { get; set; }

		public string WriterId { get; set; }
		

		// Choises of Story
		//	public ICollection<StoryChoise>? Choises { get; set; }

		
	}
}
