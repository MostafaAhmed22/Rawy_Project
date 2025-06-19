using Rawy.DAL.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models.WriterSpec
{
	public class FollowedStoriesSpec : BaseSpecifications<Story>
	{
		public FollowedStoriesSpec(List<int> followedUserIds)
		: base(s => followedUserIds.Contains(s.AppUserId))
		{
			Includes.Add(s => s.AppUser);
			Includes.Add(S => S.Comments);

			AddInclude("Comments.AppUser");
			AddOrderByDescending(s => s.CreatedAt);
		}

	}
}
