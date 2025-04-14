using Rawy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Specification
{
	public interface ISpecifications<T> where T : class
	{
		public Expression<Func<T, bool>> Criteria { get; set; }
		public List<Expression<Func<T, object>>> Includes { get; set; }
		public Expression<Func<T, object>> OrderBy { get;  set; }
		public Expression<Func<T, object>> OrderByDesc { get;  set; }

        public int Take { get; set; }
		public int Skip { get; set; }
		public bool IsPaginationsEnabled { get; set; }


		// _context.Story.include( S => S.Writer).include(S=> S.Choices)
		// _context.Story.Where(S=> S.Id == id)
	}
}
