using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Rawy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Data.Configurations
{
	public class StoryConfigurations : IEntityTypeConfiguration<Story>
	{
		public void Configure(EntityTypeBuilder<Story> builder)
		{
			builder.HasKey(s => s.Id);
			builder.Property(s => s.Id).IsRequired();

			builder.Property(s => s.Title)
				.IsRequired()
				.HasMaxLength(200);

			builder.Property(s => s.Content)
				.IsRequired();

			builder.Property(s => s.Category)
				.HasMaxLength(100);

			builder.Property(s => s.CreatedAt)
				.HasDefaultValueSql("GETDATE()");

			// Foreign Key Relationship with Choises
			//builder.HasMany(w => w.Choises)
			//	.WithOne(s => s.Story)
			//	.HasForeignKey(s => s.StoryId)
			//	.OnDelete(DeleteBehavior.ClientSetNull);

			// Foreign Key Relationship with Writer
			// Relationship: Story ↔ AppUser (Writer)
			builder.HasOne(s => s.AppUser)
				   .WithMany(u => u.Stories)
				   .HasForeignKey(s => s.AppUserId)
				   .OnDelete(DeleteBehavior.Restrict); // Prevent delete user from deleting their stories

			// Relationship: Story ↔ Comments
			builder.HasMany(s => s.Comments)
				   .WithOne(c => c.Story)
				   .HasForeignKey(c => c.StoryId)
				   .OnDelete(DeleteBehavior.Cascade);

			// Relationship: Story ↔ Ratings
			builder.HasMany(s => s.Ratings)
				   .WithOne(r => r.Story)
				   .HasForeignKey(r => r.StoryId)
				   .OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(s => s.SavedByUsers) // Assuming you have this navigation property
		   .WithOne(ss => ss.Story) 
		   .HasForeignKey(ss => ss.StoryId)
		   .OnDelete(DeleteBehavior.Cascade); // This will delete saved stories when story is d


		}
	}
}
