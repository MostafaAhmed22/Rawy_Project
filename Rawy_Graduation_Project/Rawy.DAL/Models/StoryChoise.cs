using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models
{
	public class StoryChoise
	{
		public int Id { get; set; }
		public int StoryId { get; set; }
		public string ChoiceText { get; set; }  // Example: "Go left" or "Go right"
		public int NextStoryId { get; set; }    // Links to next story part
		public Story Story { get; set; }

	}
}
