using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triolingo.Core.Entity
{
    public class StudentLesson
    {
        public int Id { get; set; }
        public double Mark { get; set; }
        public string? Note { get; set; }
        [ForeignKey("Lesson")]
        public int LessionId { get; set; }
        public virtual Lesson Lesson { get; set; }
        public virtual StudentCourse StudentCourse { get; set; }
        public int StudentCourseId { get; set; }
    }
}
