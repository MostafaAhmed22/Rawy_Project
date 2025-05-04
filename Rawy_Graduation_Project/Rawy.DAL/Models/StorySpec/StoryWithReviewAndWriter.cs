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
        public StoryWithReview(StorySpecParams specParams) : base(S=> (string.IsNullOrEmpty(specParams.Search) || S.Category.ToLower().Contains(specParams.Search))) // Search based on Category not content

        {
			Includes.Add(S => S.AppUser);
			//Includes.Add(S => S.Writer);
			Includes.Add(S => S.Comments);
            Includes.Add(S => S.Ratings);

			//Apply sorting based on the 'sort' parameter
			if (!string.IsNullOrEmpty(specParams.Sort))
			{
				//switch (specParams.Sort.ToLower())
				//{
				//	case "rateasc":
				//		AddOrderBy(S => S.Ratings.Average(r => (double?)r.Score) ?? 0);
				//		break;
				//	case "ratedesc":
				//		AddOrderByDescending(S => S.Ratings.Average(r => (double?)r.Score) ?? 0);
				//		break;
				//	default:
				//		AddOrderByDescending(S => S.CreatedAt);
				//		break;
				//}
			}
			else
			{
				AddOrderByDescending(s => s.CreatedAt); // Fallback default
			}


			//Pagination
			//pageSize = 50
			// PageIndex = 9    skip(8 * 50) and Take 50

			//ApplyPagination(specParams.Pagesize * (specParams.PageIndex - 1), specParams.Pagesize);
		}

        public StoryWithReview(int id) : base(S=> S.Id == id)
        {
			Includes.Add(S => S.AppUser);
		//	Includes.Add(S => S.Writer);
			Includes.Add(S => S.Comments);
			Includes.Add(S => S.Ratings);
		}
    }
}
