using Rawy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.BLL.Interfaces
{
  public interface IResetPasswordRepository : IGenericRepository<ResetPassword>
    {
		Task<ResetPassword> GetFirstOrDefaultAsync(Expression<Func<ResetPassword, bool>> predicate);
	}
}
