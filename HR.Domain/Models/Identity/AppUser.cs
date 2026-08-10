using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR.Domain.Models.Identity
{
    public class AppUser : IdentityUser
    {
        [Required, MaxLength(150)]
        public string FirstName { get; set; } = string.Empty;
        [Required, MaxLength(150)]
        public string LastName { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
