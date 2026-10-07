using HR.Domain.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace HR.Domain.Models.Authorization
{
 public class User_Page_Permissions
 {
  [Required]
  public string UserId { get; set; } = Guid.NewGuid ().ToString ();
  [Required]
  public int PageId { get; set; }
  [Required]
  public int PermissionId { get; set; }
  [Required]
  public bool IsAllowed { get; set; } // Indicates whether the user has permission for accessing the page or not


  public virtual Permission? Permission { get; set; }
  public virtual SubModulePages? SubModulePage { get; set; }
  public virtual AppUser? User { get; set; }

 }
}
