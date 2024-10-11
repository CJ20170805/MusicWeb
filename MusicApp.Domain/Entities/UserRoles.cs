using System;
using Microsoft.AspNetCore.Identity;

namespace MusicApp.Domain.Entities;

public class UserRoles: IdentityUserRole<Guid>
{
        public DateTime AssignedAt { get; set; }

        public UserRoles()
        {
            AssignedAt = DateTime.UtcNow;
        }

        public virtual User? User { get; set; }
        public virtual Role? Role { get; set; }
}
