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
	public class CommentConfiguration : IEntityTypeConfiguration<Comment>
	{
		public void Configure(EntityTypeBuilder<Comment> builder)
		{
			builder.HasKey(c => c.Id); // Primary Key

			builder.Property(c => c.Content)
				.IsRequired()
				.HasMaxLength(500); 

			builder.Property(c => c.CreatedAt)
				.IsRequired()
				.HasDefaultValueSql("GETDATE()"); 

			// Relationship with Story (Each comment belongs to one story)
			builder.HasOne(c => c.Story)
				.WithMany(s => s.Comments)
				.HasForeignKey(c => c.StoryId)
				.OnDelete(DeleteBehavior.Cascade);

			// Relationship with Writer (Each comment is written by one writer)
			builder.HasOne(c => c.Writer)
				.WithMany(w => w.Comments)
				.HasForeignKey(c => c.WriterId)
				.OnDelete(DeleteBehavior.Restrict); // Prevent writer deletion if ratings exist

		}
	}
}
