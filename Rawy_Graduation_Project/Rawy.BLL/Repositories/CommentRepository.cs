using Microsoft.EntityFrameworkCore;
using Rawy.BLL.Interfaces;
using Rawy.BLL.Specifications;
using Rawy.DAL.Data;
using Rawy.DAL.Models;
using Rawy.DAL.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.BLL.Repositories
{
	public class CommentRepository : GenericRepository<Comment> , ICommentRepository
	{
		private readonly RawyDBContext _context;
        public CommentRepository(RawyDBContext context) : base(context)
        {
            _context = context;
        }

		public async Task<IEnumerable<Comment>> GetCommentsByStoryIdAsync(ISpecifications<Comment> spec)
		{
			return await SpecificationsEvaluator<Comment>.GetQuery(_context.Comments, spec).ToListAsync();

		}
	}
}
