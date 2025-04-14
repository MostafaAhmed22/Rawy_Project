using Rawy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Specification
{
	public class BaseSpecifications<T> : ISpecifications<T> where T : class
	{
		public Expression<Func<T, bool>> Criteria { get; set; } = null;
		public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
		public Expression<Func<T, object>> OrderBy { get; set; } = null;
		public Expression<Func<T, object>> OrderByDesc { get; set; } = null;
		public int Take { get; set ; }
		public int Skip { get; set; }
		public bool IsPaginationsEnabled { get; set; }

		public BaseSpecifications()
        {
			//Includes = new List<Expression<Func<T, object>>>();
			//Criteria = null;

		}

        public BaseSpecifications(Expression<Func<T, bool>> CriteriaExpression)
        {
            Criteria = CriteriaExpression;
        }

		// _context.Story.include( S => S.Writer).include(S=> S.Choices)
		// _context.Story.Where(S=> S.Id == id)

		public void AddOrderBy(Expression<Func<T, object>> orderByExpression)
		{
			OrderBy = orderByExpression;
		}

		public void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
		{
			OrderByDesc = orderByDescExpression;
		}

		public void ApplyPagination(int skip , int take)
		{
			IsPaginationsEnabled = true;
			Take = take;
			Skip = skip;
		}
	}
}
