using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HR.Application.Dtos.RoleDtos
{
 public class RoleDto
 {
  public string Id { get; set; } = Guid.NewGuid().ToString();
  public string? Name { get; set; }
  public string? Description { get; set; } = string.Empty;
 }

 public class CreateRoleDto
 {
  [Required, StringLength(100)]
  public string? Name { get; set; }
  [Required, StringLength(250)]
  public string? Description { get; set; }

 }

 public class UpdateRoleDto
 {
  [Required]
  public string CurrentRoleName { get; set; } = string.Empty;
  [Required]
  public string NewName { get; set; } = string.Empty;
  [Required]
  public string Description { get; set; } = string.Empty;
 }
}
