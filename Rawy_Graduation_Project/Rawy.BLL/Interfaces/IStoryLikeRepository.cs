using Rawy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.BLL.Interfaces
{
   public interface IStoryLikeRepository :IGenericRepository<StoryLike>
    {
		Task<StoryLike> GetUserReactionAsync(int storyId, int userId); // to know if user react this story or not
		Task<int> CountLikesAsync(int storyId);
		Task<int> CountDislikesAsync(int storyId);
	}
}
