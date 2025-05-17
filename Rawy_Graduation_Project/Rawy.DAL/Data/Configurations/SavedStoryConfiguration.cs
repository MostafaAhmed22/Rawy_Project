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
	public class SavedStoryConfiguration : IEntityTypeConfiguration<SavedStory>
	{
		public void Configure(EntityTypeBuilder<SavedStory> builder)
		{
			builder
		 .HasKey(ss => new { ss.UserId, ss.StoryId });

			// علاقة SavedStory → User
			builder
				.HasOne(ss => ss.User)
				.WithMany(u => u.SavedStories)
				.HasForeignKey(ss => ss.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			// علاقة SavedStory → Story
			builder
				.HasOne(ss => ss.Story)
				.WithMany(s => s.SavedByUsers)
				.HasForeignKey(ss => ss.StoryId)
				.OnDelete(DeleteBehavior.Restrict); 

		}
	}
}
