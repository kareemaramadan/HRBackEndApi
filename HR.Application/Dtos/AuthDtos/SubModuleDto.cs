using System.ComponentModel.DataAnnotations;

namespace HR.Application.Dtos.AuthDtos
{
 public class SubModuleDto
 {
  public int? SubModuleId { get; set; }
  [Required]
  public string SubModuleName_en { get; set; } = string.Empty;
  [Required]
  public string SubModuleName_ar { get; set; } = string.Empty;
 }


}
