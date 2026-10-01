using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HR.Application.Dtos.AuthDtos
{
 public class ChangePasswordDto
 {
  [Required]
  public string Username { get; set; }
  [Required]
  public string OldPassword { get; set; }
  [Required]
  public string NewPassword { get; set; }
  [Required]
  public string ConfirmPassword { get; set; }

 }
}
