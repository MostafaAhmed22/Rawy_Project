using Rawy.DAL.Models;
using Rawy.DAL.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.BLL.Interfaces
{
  public interface ICommentRepository : IGenericRepository<Comment>
    {
		Task<IEnumerable<Comment>> GetCommentsByStoryIdAsync(ISpecifications<Comment> spec);

	}
}
