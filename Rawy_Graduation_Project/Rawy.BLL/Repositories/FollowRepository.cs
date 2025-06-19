using Microsoft.EntityFrameworkCore;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Data;
using Rawy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.BLL.Repositories
{
	public class FollowRepository:GenericRepository<WriterFollow>,IFollowRepository
	{
		private readonly RawyDBContext _context;
		public FollowRepository(RawyDBContext context) : base(context)
        {
			_context = context;
		}
		public async Task<List<int>> GetFollowedUserIdsAsync(int currentUserId)
		{
			return await _context.WriterFollows
				.Where(f => f.FollowerId == currentUserId)
				.Select(f => f.FolloweeId)
				.ToListAsync(); 
		}
	}
}
