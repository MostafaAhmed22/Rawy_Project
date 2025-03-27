using Rawy.DAL.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models.StorySpec
{
   public class RatingOfStorySpec : BaseSpecifications<Rating>
    {
		public RatingOfStorySpec() : base()

		{
			Includes.Add(C => C.Writer);
			Includes.Add(C => C.Story);

		}

		public RatingOfStorySpec(int id) : base(C => C.StoryId == id)
		{
			Includes.Add(C => C.Writer);
			Includes.Add(C => C.Story);
		}
	}
}
