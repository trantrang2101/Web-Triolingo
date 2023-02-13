using System;
using System.Collections.Generic;

namespace Web_Triolingo.Model
{
    public partial class Lesson
    {
        public Lesson()
        {
            Questions = new HashSet<Question>();
            StudentLessons = new HashSet<StudentLesson>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int UnitId { get; set; }
        public string? Description { get; set; }
        public string? Note { get; set; }
        public int Status { get; set; }

        public virtual Unit Unit { get; set; } = null!;
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<StudentLesson> StudentLessons { get; set; }
    }
}
