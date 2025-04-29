using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models
{
	public class WriterFollow
	{
		public int FollowerId { get; set; }     // The one who follows
		public Writer Follower { get; set; }

		public int FolloweeId { get; set; }     // The one being followed
		public Writer Followee { get; set; }

		public DateTime FollowedAt { get; set; } = DateTime.UtcNow;
	}
}
