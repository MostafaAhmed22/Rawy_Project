﻿using Rawy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.BLL.Interfaces
{
	public interface IGenericRepository<T> where T : BaseEntity
	{
			Task<IEnumerable<T>> GetAllAsync();
			Task<T?> GetByIdAsync(int id);
			Task AddAsync(T entity);
			Task UpdateAsync(T entity);
			Task DeleteAsync(int id);
	}
}
