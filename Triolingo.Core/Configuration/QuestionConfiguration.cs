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
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Question");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Question1).HasMaxLength(250).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Mark).IsRequired();
        }
    }
}
