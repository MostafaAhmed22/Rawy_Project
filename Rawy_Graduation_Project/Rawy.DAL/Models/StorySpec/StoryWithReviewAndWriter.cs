using Rawy.DAL.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models.StorySpec
{
    public class StoryWithReview : BaseSpecifications<Story>
    {
        public StoryWithReview() : base()

        {
			Includes.Add(S => S.Writer);
            Includes.Add(S => S.Comments);
            Includes.Add(S => S.Ratings);
		}

        public StoryWithReview(string id) : base(S=> S.Id == id)
        {
			Includes.Add(S => S.Writer);
			Includes.Add(S => S.Comments);
			Includes.Add(S => S.Ratings);
		}
    }
}
