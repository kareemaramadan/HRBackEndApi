using HR.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Domain.Models.Authorization
{
    public class RolePagePermission
    {
        public string RoleId { get; set; } = Guid.NewGuid().ToString();
        public int PageId { get; set; } 
        public int PermissionId { get; set; }


        public virtual AppRole? Roles { get; set; }
        public virtual ModulePage? ModulePages { get; set; }
        public virtual Permission? Permissions { get; set; }

    }
}
