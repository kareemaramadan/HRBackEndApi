using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Application.Dtos.AuthDtos
{
 public class ModulePagesDto
 {
  public int? PageId { get; set; }
  public int ModuleId { get; set; } = 0;
  public string PageName_en { get; set; } = string.Empty;
  public string PageName_ar { get; set; } = string.Empty;
  public string PageUrl { get; set; } = string.Empty;
 }

 public class CreateModulePagesDto {

  public int ModuleId { get; set; } = 0;
  public string PageName_en { get; set; } = string.Empty;
  public string PageName_ar { get; set; } = string.Empty;
  public string PageUrl { get; set; } = string.Empty;

 }


}
