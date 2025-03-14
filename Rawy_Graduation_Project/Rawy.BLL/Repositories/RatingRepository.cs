using Microsoft.EntityFrameworkCore;
using Rawy.BLL.Interfaces;
using Rawy.BLL.Specifications;
using Rawy.DAL.Data;
using Rawy.DAL.Models;
using Rawy.DAL.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.BLL.Repositories
{
	public class RatingRepository : GenericRepository<Rating> , IRatingRepository
	{
		private readonly RawyDBContext _context;

		public RatingRepository(RawyDBContext context) : base(context)
		{
			_context = context;
		}
		public async Task<IEnumerable<Rating>> GetRatingByStoryIdAsync(ISpecifications<Rating> spec)
		{
			return await SpecificationsEvaluator<Rating>.GetQuery(_context.Ratings, spec).ToListAsync();

		}

		public async Task<double> GetAverageRatingByStoryIdAsync(string storyId)
		{
			var ratings = await _context.Ratings
				.Where(r => r.StoryId == storyId)
				.Select(r => r.Score)
				.ToListAsync();

			return ratings.Any() ? ratings.Average() : 0;
		}

		public async Task AddRatingAsync(Rating rating)
		{
			// Check if the writer has already rated this story
			var existingRating = await _context.Ratings
				.FirstOrDefaultAsync(r => r.WriterId == rating.WriterId && r.StoryId == rating.StoryId);

			if (existingRating != null)
			{
				throw new InvalidOperationException("You have already rated this story.");
			}

			await _context.Ratings.AddAsync(rating);
			await _context.SaveChangesAsync();
		}
	}
}
