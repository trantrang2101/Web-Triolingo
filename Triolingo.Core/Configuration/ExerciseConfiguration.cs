using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triolingo.Core.Entity;

namespace Triolingo.Core.Configuration
{
    public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.ToTable("Exercise");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Status).IsRequired();
            builder.Property(e => e.Title).HasMaxLength(250).IsRequired();
            builder.Property(e => e.Description).IsRequired();
            builder.Property(e => e.File).IsRequired();
        }
    }
}
