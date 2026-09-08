using HR.Domain.Models.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HR.Domain.Models.Identity
{ 
    public class AppRole : IdentityRole
    {
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Description { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }



        public virtual ICollection<RolePagePermission>? RolePagePermissions { get; set; }

    }
}
