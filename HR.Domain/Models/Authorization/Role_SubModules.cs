using HR.Domain.Models.Identity;

namespace HR.Domain.Models.Authorization
{
 public class Role_SubModules
 {
  public string RoleId { get; set; } = Guid.NewGuid ().ToString ();
  public int ModuleId { get; set; }
  public int SubModuleId { get; set; }


  public virtual AppRole? Role { get; set; }
  public virtual Module? Module { get; set; }
  public virtual SubModules? SubModule { get; set; }

 }
}
