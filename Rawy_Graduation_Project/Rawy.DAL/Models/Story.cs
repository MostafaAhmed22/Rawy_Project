using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rawy.DAL.Models
{
	public class Story : BaseEntity
	{
      //  public string Id { get; set; } = Guid.NewGuid().ToString();
		public string Title { get; set; }
		public string Content { get; set; }
		public string Category { get; set; }
		public DateTime CreatedAt { get; set; }

		// Comments
		public ICollection<Comment> Comments { get; set; }

		//Rating 
		public ICollection<Rating> Ratings { get; set; }

		// Choises of Story
		//	public ICollection<StoryChoise> Choises { get; set; }

		//public int WriterId { get; set; }
		//[JsonIgnore]
		//public Writer Writer { get; set; }

		public int AppUserId { get; set; }
		public AppUser AppUser { get; set; }

	}

}
