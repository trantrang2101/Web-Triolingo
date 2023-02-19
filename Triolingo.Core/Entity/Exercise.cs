﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triolingo.Core.Entity
{
    public class Exercise
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual Setting Setting { get; set; }
        public int TypeId { get; set; }
        public virtual Lesson Lesson { get; set; }
        public int LessonId { get; set; }
        public string File { get; set; }
    }
}