using Rawy.DAL.Models;
using Rawy.DAL.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.BLL.Interfaces
{
	public interface IRatingRepository : IGenericRepository<Rating>
	{
		Task<IEnumerable<Rating>> GetRatingByStoryIdAsync(ISpecifications<Rating> spec);
		Task<double> GetAverageRatingByStoryIdAsync(string storyId);
		Task AddRatingAsync(Rating rating);
	}
}
