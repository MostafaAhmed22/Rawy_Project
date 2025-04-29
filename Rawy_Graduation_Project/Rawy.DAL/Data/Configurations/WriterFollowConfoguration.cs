using Microsoft.EntityFrameworkCore;
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
	public class WriterFollowConfoguration : IEntityTypeConfiguration<WriterFollow>
	{
		public void Configure(EntityTypeBuilder<WriterFollow> builder)
		{
			builder.HasKey(f => new { f.FollowerId, f.FolloweeId });

			builder.HasOne(f => f.Follower)
				   .WithMany(w => w.Followings)
				   .HasForeignKey(f => f.FollowerId)
				   .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

			builder.HasOne(f => f.Followee)
				   .WithMany(w => w.Followers)
				   .HasForeignKey(f => f.FolloweeId)
				   .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
		}
	}
}
