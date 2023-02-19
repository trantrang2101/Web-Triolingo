using System;
using System.Collections.Generic;

namespace Triolingo.Core.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public int Status { get; set; }
        public string? Note { get; set; }
    }
}
