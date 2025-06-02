using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models
{
	public class StoryLike
	{
		public int Id { get; set; }
        public bool IsLike { get; set; }  // true => like  false => dislike

        public int AppUserId { get; set; }
		public AppUser AppUser { get; set; }

		public int StoryId { get; set; }
		public Story Story { get; set; }
	}
}
