using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rawy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Data.Configurations
{
	public class StoryLikeConfiguration : IEntityTypeConfiguration<StoryLike>
	{
		public void Configure(EntityTypeBuilder<StoryLike> builder)
		{
			builder.HasKey(r => r.Id);

			builder.HasOne(r => r.AppUser)
				   .WithMany(u => u.StoryLikes)
				   .HasForeignKey(r => r.AppUserId)
				   .OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(r => r.Story)
				   .WithMany(s => s.StoryLikes)
				   .HasForeignKey(r => r.StoryId)
				   .OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(r => new { r.AppUserId, r.StoryId }).IsUnique(); // Prevents duplicate reactions
		}
	}
}
