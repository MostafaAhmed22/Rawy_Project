using Rawy.DAL.Models;
using System.Collections;

namespace Rawy.APIs.Dtos.StoryDtos
{
	public class UpdateStoryDto
	{
		public string Title { get; set; }
		public string Content { get; set; }
		public string Category { get; set; }
		public ICollection<ChoiseDto> Choices { get; set; }
	}
}
