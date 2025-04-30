﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rawy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Data.Configurations
{
	public class RatingConfiguration : IEntityTypeConfiguration<Rating>
	{
		public void Configure(EntityTypeBuilder<Rating> builder)
		{
			builder.HasKey(r => r.Id); // Primary Key

			builder.Property(r => r.Score)
				.IsRequired()
				.HasDefaultValue(1); 

			builder.Property(r => r.CreatedAt)
				.IsRequired()
				.HasDefaultValueSql("GETDATE()");


			builder.HasIndex(r => new { r.AppUserId, r.StoryId })
		   .IsUnique(); // Ensures one Writer can rate a Story only once

			// Relationship with Story (Each rating belongs to a single story)
			builder.HasOne(r => r.Story)
				.WithMany(s => s.Ratings)
				.HasForeignKey(r => r.StoryId)
				.OnDelete(DeleteBehavior.Cascade); // Cascade delete when Story is deleted

			// Relationship with Writer (Each rating is given by one writer)
			builder.HasOne(r => r.AppUser)
				.WithMany(w => w.Ratings)
				.HasForeignKey(r => r.AppUserId)
				.OnDelete(DeleteBehavior.Restrict); // Prevent writer deletion if ratings exist
		}
	}
}
