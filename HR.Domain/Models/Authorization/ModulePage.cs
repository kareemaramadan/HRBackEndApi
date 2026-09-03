using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Domain.Models.Authorization
{
    public class ModulePage
    {
        public int PageId { get; set; }
        public int ModuleId { get; set; } = 0;
        public string PageName { get; set; } = string.Empty;
        public string PageUrl { get; set; } = string.Empty;


        public virtual Module? Modules { get; set; }
        public virtual ICollection<RolePagePermission>? RolePagePermissions { get; set; }
        public virtual ICollection<UserPagePermission>? UserPagePermissions { get; set; }

    }
}
