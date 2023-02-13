using System;
using System.Collections.Generic;

namespace Web_Triolingo.Model
{
    public partial class Setting
    {
        public Setting()
        {
            InverseParent = new HashSet<Setting>();
            Questions = new HashSet<Question>();
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Status { get; set; }
        public string? Note { get; set; }
        public string Value { get; set; } = null!;
        public int? ParentId { get; set; }

        public virtual Setting? Parent { get; set; }
        public virtual ICollection<Setting> InverseParent { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
