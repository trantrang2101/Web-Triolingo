using System;
using System.Collections.Generic;

namespace Web_Triolingo.Models
{
    public partial class UserRole
    {
        public int Id { get; set; }
        public string? Note { get; set; }
        public int UserId { get; set; }
        public int RoleType { get; set; }

        public virtual Setting RoleTypeNavigation { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
