﻿using Rawy.DAL.Models;

namespace Rawy.APIs.Dtos.StoryDtos
{
	public class AddStoryDto
	{
		public string Title { get; set; }
		public string Content { get; set; }
		public string Category { get; set; }
		//public DateTime CreatedAt { get; set; }

		// Choises of Story
	//	public ICollection<StoryChoise>? Choises { get; set; }

		//public int WriterId { get; set; }
		//public Writer Writer { get; set; }
	}
}
