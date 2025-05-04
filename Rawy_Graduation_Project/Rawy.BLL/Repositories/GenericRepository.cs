﻿using Microsoft.EntityFrameworkCore;
using Rawy.BLL.Interfaces;
using Rawy.BLL.Specifications;
using Rawy.DAL.Data;
using Rawy.DAL.Models;
using Rawy.DAL.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.BLL.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly RawyDBContext _context;

		public GenericRepository(RawyDBContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
		{

			return await SpecificationsEvaluator<T>.GetQuery(_context.Set<T>(), spec).ToListAsync();

			 //_context.Set<T>().ToListAsync();
		}

		public async Task<T?> GetByIdWithSpecAsync(ISpecifications<T> spec)
		{
			return await SpecificationsEvaluator<T>.GetQuery(_context.Set<T>(), spec).FirstOrDefaultAsync();
		}
		public async Task AddAsync(T entity)
		{
			_context.Set<T>().Add(entity);
			//await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var entity = await _context.Set<T>().FindAsync(id).AsTask(); // Convert ValueTask<T?> to Task<T?>
			if (entity != null)
			{
				_context.Set<T>().Remove(entity);
				//await _context.SaveChangesAsync();
			}

			
		}


		public async Task UpdateAsync(T entity)
		{
			_context.Set<T>().Update(entity);
			
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _context.Set<T>().ToListAsync();
		}

		public async Task<T?> GetByIdAsync(int id)
		{
			return await _context.Set<T>().FindAsync(id);
		}

		public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
		{
			return await _context.Set<T>().FirstOrDefaultAsync(predicate);
		}

		public async Task Delete(T entity)
		{
			_context.Set<T>().Remove(entity);
		}
	}
}
