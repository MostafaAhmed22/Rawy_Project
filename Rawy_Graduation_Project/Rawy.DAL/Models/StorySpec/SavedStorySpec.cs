using Rawy.DAL.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models.StorySpec
{
	public class SavedStorySpec : BaseSpecifications<SavedStory>
	{
			public SavedStorySpec(int userId) : base(ss => ss.UserId == userId)
			{
			Includes.Add(ss => ss.Story.AppUser);
			Includes.Add(ss => ss.Story);
				Includes.Add(ss => ss.User);
				AddOrderByDescending(ss => ss.SavedAt);
			}

		
	}	
}
