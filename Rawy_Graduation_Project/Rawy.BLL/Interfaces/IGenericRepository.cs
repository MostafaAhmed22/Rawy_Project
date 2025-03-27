using Rawy.DAL.Models;
using Rawy.DAL.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.BLL.Interfaces
{
	public interface IGenericRepository<T> where T : class
	{
			Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
			Task<T?> GetByIdWithSpecAsync(ISpecifications<T> spec);
		    Task<IEnumerable<T>> GetAllAsync();
			Task<T?> GetByIdAsync(int id);
			Task AddAsync(T entity);
			Task UpdateAsync(T entity);
			Task DeleteAsync(int id);
	}
}
