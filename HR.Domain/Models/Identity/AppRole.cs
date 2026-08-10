using Microsoft.AspNetCore.Identity;

namespace HR.Domain.Models.Identity
{ 
    public class AppRole : IdentityRole
    {
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
