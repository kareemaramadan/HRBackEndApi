using HR.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HR.Domain.Models.Authorization
{
    public class UserPagePermission
    {
        [Required]
        public string UserId { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public int PageId { get; set; }
        [Required]
        public int PermissionId { get; set; }
        [Required]
        public bool IsAllowed { get; set; } // Indicates whether the user has permission for accessing the page or not


        public virtual Permission? Permissions { get; set; }
        public virtual ModulePage? Pages { get; set; }
        public virtual AppUser? Users { get; set; }

    }
}
