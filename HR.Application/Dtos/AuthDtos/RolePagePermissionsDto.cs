using HR.Domain.Models.Authorization;
using HR.Domain.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace HR.Application.Dtos.AuthDtos
{
 public class RolePagePermissionsDto ( )
 {
  public string RoleId { get; set; } = Guid.NewGuid ().ToString ();
  public int ModuleId { get; set; }
  public int PageId { get; set; }
  public int PermissionId { get; set; }

  public AppRole? Roles { get; set; }
  public SubModulePages? SubModulePages { get; set; }
  public Permission? Permissions { get; set; }

 }

 public class GetRolePagePermissionsDto
 {

  [Required]
  public string RoleId { get; set; } = Guid.NewGuid ().ToString ();
  [Required]
  public int ModuleId { get; set; }
 }


}
