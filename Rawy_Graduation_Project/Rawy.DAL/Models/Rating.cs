using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models
{
	public class Rating : BaseEntity
	{
        // Score between [1,5]
     //   public string Id { get; set; } = Guid.NewGuid().ToString();
		public int Score { get; set; }

		public DateTime CreatedAt { get; set; }

		// Relationships
		//public int WriterId { get; set; }
		//public Writer Writer { get; set; }

		public int AppUserId { get; set; }
		public AppUser AppUser { get; set; }

		public int StoryId { get; set; }
		public Story Story { get; set; }
	}
}
