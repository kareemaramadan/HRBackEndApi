namespace HR.Domain.Models.Authorization
{
 public class Permission
 {
  public int PermissionId { get; set; }
  public string PermissionName { get; set; } = string.Empty;



  public virtual ICollection<Role_Page_Permissions>? Role_Page_Permissions { get; set; }
  public virtual ICollection<User_Page_Permission>? User_Page_Permissions { get; set; }

 }
}
