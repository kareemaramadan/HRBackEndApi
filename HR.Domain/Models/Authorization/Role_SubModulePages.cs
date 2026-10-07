using HR.Domain.Models.Identity;

namespace HR.Domain.Models.Authorization
{
 public class Role_SubModulePages
 {

  public string? RoleId { get; set; }
  public int PageId { get; set; }

  public virtual AppRole? Role { get; set; }
  public virtual SubModulePages? SubModulePage { get; set; }



 }
}
