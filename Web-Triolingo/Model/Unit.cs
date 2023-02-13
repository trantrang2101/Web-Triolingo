using System;
using System.Collections.Generic;

namespace Web_Triolingo.Model
{
    public partial class Unit
    {
        public Unit()
        {
            Lessons = new HashSet<Lesson>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Order { get; set; }
        public int CourseId { get; set; }
        public int Status { get; set; }
        public string? Note { get; set; }

        public virtual Course Course { get; set; } = null!;
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
