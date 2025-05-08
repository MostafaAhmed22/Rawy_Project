using Microsoft.EntityFrameworkCore;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Data;
using Rawy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.BLL.Repositories
{
	public class ResetPasswordRepository : GenericRepository<ResetPassword>, IResetPasswordRepository
	{
		private readonly RawyDBContext _context;
		public ResetPasswordRepository(RawyDBContext context) : base(context)
		{
			_context = context;
		}
		public async Task<ResetPassword> GetFirstOrDefaultAsync(Expression<Func<ResetPassword, bool>> predicate)
		{
			return await _context.PasswordResetCodes.FirstOrDefaultAsync(predicate);
		}

	
	}
}
