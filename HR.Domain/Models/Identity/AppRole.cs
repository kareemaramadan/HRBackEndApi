using HR.Domain.Models.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HR.Domain.Models.Identity
{ 
    public class AppRole : IdentityRole
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Description { get; set; } = string.Empty;
        public virtual ICollection<RolePagePermission>? RolePagePermissions { get; set; }

    }
}
