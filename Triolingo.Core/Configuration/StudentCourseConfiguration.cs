﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triolingo.Core.Entity;

namespace Triolingo.Core.Configuration
{
    public class StudentCourseConfiguration : IEntityTypeConfiguration<StudentCourse>
    {
        public void Configure(EntityTypeBuilder<StudentCourse> builder)
        {
            builder.ToTable("StudentCourse");
            builder.HasKey(t => t.Id);
            builder.Property(x => x.Comment);
            builder.Property(x => x.Rate);
            builder.Property(x=>x.IsStudent).HasDefaultValue(true);
        }
    }
}
