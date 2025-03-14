using Rawy.DAL.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models.StorySpec
{
	public class CommentsOfStorySpec : BaseSpecifications<Comment>
	{
		public CommentsOfStorySpec() : base()

		{
			Includes.Add(C => C.Writer);
			Includes.Add(C=>C.Story);

		}

		public CommentsOfStorySpec(string id) : base(C=>C.StoryId == id)
		{
			Includes.Add(C => C.Writer);
			Includes.Add(C => C.Story);
		}
	}
}
