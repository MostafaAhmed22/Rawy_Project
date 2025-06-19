//using Microsoft.EntityFrameworkCore;
//using Rawy.BLL.Interfaces;
//using Rawy.DAL.Data;
//using Rawy.DAL.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Rawy.BLL.Repositories
//{
//	public class StoryLikeRepository : GenericRepository<StoryLike>, IStoryLikeRepository
//	{
//		private readonly RawyDBContext _context;
//		public StoryLikeRepository(RawyDBContext context) : base(context)
//		{
//			_context = context;
//		}

//		public async Task<int> CountDislikesAsync(int storyId)
//		{
//			return await _context.Likes.CountAsync(l=>l.StoryId == storyId&&!l.IsLike);
//		}

//		public async Task<int> CountLikesAsync(int storyId)
//		{
//			return await _context.Likes.CountAsync(l=>l.StoryId == storyId && l.IsLike);
//		}

//		public async Task<StoryLike> GetUserReactionAsync(int storyId, int userId)
//		{
//			return await _context.Likes.FirstOrDefaultAsync(l => l.StoryId == storyId && l.AppUserId == userId);

//		}
//	}
//}
