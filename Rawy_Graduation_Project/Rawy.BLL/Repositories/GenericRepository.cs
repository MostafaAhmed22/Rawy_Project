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
	internal class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly RawyDBContext _context;

		public GenericRepository(RawyDBContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _context.Set<T>().ToListAsync();
		}

		public async Task<T?> GetByIdAsync(int id)
		{
			return await _context.Set<T>().FindAsync(id);
		}
		public async Task AddAsync(T entity)
		{
			_context.Set<T>().Add(entity);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var entity = await _context.Set<T>().FindAsync(id).AsTask(); // Convert ValueTask<T?> to Task<T?>
			if (entity != null)
			{
				_context.Set<T>().Remove(entity);
				await _context.SaveChangesAsync();
			}

			
		}


		public async Task UpdateAsync(T entity)
		{
			_context.Set<T>().Update(entity);
			await _context.SaveChangesAsync();
		}
	}
}
