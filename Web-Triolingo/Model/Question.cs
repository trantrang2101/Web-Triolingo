using System;
using System.Collections.Generic;

namespace Web_Triolingo.Model
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public int Id { get; set; }
        public string Question1 { get; set; } = null!;
        public string? Description { get; set; }
        public int Status { get; set; }
        public int TypeId { get; set; }
        public int LessonId { get; set; }
        public string? File { get; set; }
        public int Mark { get; set; }

        public virtual Lesson Lesson { get; set; } = null!;
        public virtual Setting Type { get; set; } = null!;
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
