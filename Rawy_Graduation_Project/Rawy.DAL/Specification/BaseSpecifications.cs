using Rawy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Specification
{
	public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
	{
		public Expression<Func<T, bool>> Criteria { get; set; } = null;
		public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();

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
	}
}
