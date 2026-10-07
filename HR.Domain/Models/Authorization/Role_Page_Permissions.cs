using HR.Domain.Models.Identity;

namespace HR.Domain.Models.Authorization
{
 public class Role_Page_Permissions
 {
  public string RoleId { get; set; } = Guid.NewGuid ().ToString ();
  public int PageId { get; set; }
  public int PermissionId { get; set; }


  public virtual AppRole? Role { get; set; }
  public virtual SubModulePages? SubModulePage { get; set; }
  public virtual Permission? Permission { get; set; }

 }
}
