using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models
{
	public class WriterFollow
	{
		public int FollowerId { get; set; }
		//public Writer Follower { get; set; }// The one who follows
		public AppUser Follower { get; set; }

		public int FolloweeId { get; set; }
	//	public Writer Followee { get; set; }// The one being followed
		public AppUser Followee { get; set; }

		public DateTime FollowedAt { get; set; } = DateTime.UtcNow;
	}
}
