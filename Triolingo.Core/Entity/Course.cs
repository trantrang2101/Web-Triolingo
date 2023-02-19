using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triolingo.Core.Entity
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double? RateAverage { get; set; }
        public int Status { get; set; }
        public string? Note { get; set; }
    }
}
