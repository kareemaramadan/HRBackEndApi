using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HR.Application.Dtos.AuthDtos
{
    public class PermissionDto
    {
        public int? PermissionId { get; set; }
        [Required]
        public string PermissionName { get; set; } = string.Empty;
    }

 public class CreatePermissionDto
 {
  [Required]
  public string PermissionName { get; set; } = string.Empty;
 }

}
