using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models
{
    public class StoryReview 
    {
		public string Id { get; set; }
		public string Comment { get; set; }
		public int Rating { get; set; } 
		public DateTime CreatedAt { get; set; }

		// Foreign Key for Story
		public string StoryId { get; set; }
		public Story Story { get; set; }

		// Foreign Key for Writer (Reviewer)
		public string WriterId { get; set; }
		public Writer Writer { get; set; }
	}
}
