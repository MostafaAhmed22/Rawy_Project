﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rawy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Data
{
	public class RawyDBContext : IdentityDbContext<AppUser,IdentityRole<int>,int>
	{

		public RawyDBContext(DbContextOptions<RawyDBContext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            modelBuilder.Entity<WriterFollow>()
                .HasKey(ef => new { ef.FollowerId, ef.FollowedId }); // Composite Key

            modelBuilder.Entity<WriterFollow>()
                .HasOne(ef => ef.Follower)
                .WithMany(e => e.Following)
                .HasForeignKey(ef => ef.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WriterFollow>()
                .HasOne(ef => ef.Followed)
                .WithMany(e => e.Followers)
                .HasForeignKey(ef => ef.FollowedId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.ApplyConfiguration(new StoryConfigurations());
            //modelBuilder.ApplyConfiguration(new WriterConfigurations());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			base.OnModelCreating(modelBuilder);
		}

		public DbSet<Story> Stories { get; set; }
		public DbSet<Writer> Writers { get; set; }
        public DbSet<Comment> Comments { get; set; }
		public DbSet<Rating> Ratings { get; set; }

	}
}
