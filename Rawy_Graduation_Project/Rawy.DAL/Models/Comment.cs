using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models
{
	public class Comment : BaseEntity
	{
	//	public string Id { get; set; }// = Guid.NewGuid().ToString();
		public string Content { get; set; }
		public DateTime CreatedAt { get; set; }

		// Relationships
		public int WriterId { get; set; }
		public Writer Writer { get; set; }

		public int StoryId { get; set; }
		public Story Story { get; set; }
	}
}
