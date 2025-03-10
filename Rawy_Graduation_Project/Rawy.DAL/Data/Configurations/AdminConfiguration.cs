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
	public class AdminConfiguration : IEntityTypeConfiguration<Admin>
	{
		public void Configure(EntityTypeBuilder<Admin> builder)
		{
			builder.HasKey(A => A.AdminId);

			// Set AdminId as Foreign Key from AppUser
			builder.HasOne(A => A.AppUser)
				   .WithOne(U => U.Admin)
				   .HasForeignKey<Admin>(A => A.AdminId)
				   .OnDelete(DeleteBehavior.Cascade); // Cascade delete when user is deleted

			// Configure properties
			builder.Property(w => w.FName)
				   .HasMaxLength(50)
				   .IsRequired();

			builder.Property(w => w.LName)
				   .HasMaxLength(50)
				   .IsRequired();
		}
	}
}
