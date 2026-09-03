namespace HR.Domain.Models.Authorization
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; } = string.Empty;
        public string PermissionCode { get; set; } = string.Empty;


        public virtual ICollection<RolePagePermission>? RolePagePermissions { get; set; }
        public virtual ICollection<UserPagePermission>? UserPagePermissions { get; set; }

    }
}
