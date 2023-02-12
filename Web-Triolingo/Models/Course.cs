using System;
using System.Collections.Generic;

namespace Web_Triolingo.Models
{
    public partial class Course
    {
        public Course()
        {
            StudentCourses = new HashSet<StudentCourse>();
            Units = new HashSet<Unit>();
        }

        public int Id { get; set; }
        public string? Name { get; set; } = null!;
        public string Description { get; set; }
        public double? RateAverage { get; set; }
        public int Status { get; set; }
        public string? Note { get; set; } = null!;

        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
        public virtual ICollection<Unit> Units { get; set; }

        public static implicit operator Task<object>(Course v)
        {
            throw new NotImplementedException();
        }
    }
}
