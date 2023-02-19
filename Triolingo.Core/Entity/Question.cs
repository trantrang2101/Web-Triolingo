using System;
using System.Collections.Generic;

namespace Triolingo.Core.Entity
{
    public class Question
    {
        public int Id { get; set; }
        public string Question1 { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public Setting Type { get; set; }
        public int TypeId { get; set; }
        public Lesson Lesson { get; set; }
        public int LessonId { get; set; }
        public string? File { get; set; }
        public int Mark { get; set; }

    }
}
