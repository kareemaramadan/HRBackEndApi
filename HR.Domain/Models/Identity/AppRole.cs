using HR.Domain.Models.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HR.Domain.Models.Identity
{
 public class AppRole : IdentityRole
 {
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public string Description { get; set; } = string.Empty;


  public virtual ICollection<Role_Page_Permissions>? Role_Page_Permissions { get; set; }
  public virtual ICollection<Role_SubModulePages>? Role_SubModulePages { get; set; }
  public virtual ICollection<Role_SubModules>? Role_SubModules { get; set; }
 }
}
