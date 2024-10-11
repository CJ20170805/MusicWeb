using System;
using Microsoft.AspNetCore.Identity;

namespace MusicApp.Domain.Entities;

public class Role: IdentityRole<Guid>
{
    public Role() : base() {
        UserRoles = new HashSet<UserRoles>();
     }
    public Role(string roleName) : base(roleName) { 
        UserRoles = new HashSet<UserRoles>();
    }
    
    public virtual ICollection<UserRoles> UserRoles { get; set; }
}
