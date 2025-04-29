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
	public class WriterConfiguration : IEntityTypeConfiguration<Writer>
	{
		public void Configure(EntityTypeBuilder<Writer> builder)
		{
			// Set WriterId as Primary Key
			builder.HasKey(w => w.WriterId);

			// Set WriterId as Foreign Key from AppUser
			builder.HasOne(w => w.AppUser)
				   .WithOne(U=>U.Writer)
				   .HasForeignKey<Writer>(w => w.WriterId)
				   .OnDelete(DeleteBehavior.Cascade); // Cascade delete when user is deleted

			// Configure properties
			builder.Property(w => w.FName)
				   .HasMaxLength(50)
				   .IsRequired();

			builder.Property(w => w.LName)
				   .HasMaxLength(50)
				   .IsRequired();

		

			// relationship with Story
			builder.HasMany(w => w.Stories)
				   .WithOne(s => s.Writer)
				   .HasForeignKey(s => s.WriterId)
				   .OnDelete(DeleteBehavior.Cascade); // Delete all stories if the writer is deleted

			// Map to table
			builder.ToTable("Writers");
		}
	}
}
