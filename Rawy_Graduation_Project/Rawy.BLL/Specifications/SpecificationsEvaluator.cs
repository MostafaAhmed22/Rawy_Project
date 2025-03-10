using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Rawy.DAL.Models;
using Rawy.DAL.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.BLL.Specifications
{
	public class SpecificationsEvaluator<TEntity> where TEntity : class
	{
		public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> baseQuery ,ISpecifications<TEntity> spec)
		{
			var query = baseQuery;   // _context.Set<T>
			if(spec.Criteria is not null)
			{
				query = query.Where(spec.Criteria);
				//_context.Set<T>.where(S=> S.id = ID)   spec.criteria=> lambdaExpression
			}

			query = spec.Includes.Aggregate(query,(currentQuery,includeExpression) => currentQuery.Include(includeExpression));
			// loop on every include in Includes and Add It To base Query
			return query;

		}
	}

	// _context.Story.include( S => S.Writer).include(S=> S.Choices)
	// _context.Story.Where(S=> S.Id == id)
}
