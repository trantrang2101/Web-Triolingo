﻿using System;
using System.Collections.Generic;

namespace Triolingo.Core.Entity
{
    public partial class StudentCourse
    {
        public int Id { get; set; }
        public User Student { get; set; }
        public int StudentId { get; set; }
        public Course Course { get; set; }
        public int CourseId { get; set; }
        public double? Rate { get; set; }
        public string? Comment { get; set; }
    }
}
