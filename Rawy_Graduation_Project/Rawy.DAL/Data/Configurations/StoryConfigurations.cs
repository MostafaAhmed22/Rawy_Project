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
			//builder.HasOne(s => s.Writer)
			//	.WithMany(w => w.Stories)
			//	.HasForeignKey(s => s.WriterId)
			//	.OnDelete(DeleteBehavior.ClientSetNull);
		}
	}
}
