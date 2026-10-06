using HR.Domain.Models.Authorization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HR.Application.Dtos.AuthDtos
{
 public class ModulePagesDtowithModule (ModulePage mp , string language)
 {
  public int ModuleId { get; set; } = mp.ModuleId;
  public string ModuleName { get; set; } = language == "en" ? mp.Modules!.ModuleName_en : mp.Modules!.ModuleName_ar;
  public int? PageId { get; set; } = mp.PageId;
  public string PageName_en { get; set; } = mp.PageName_en;
  public string PageName_ar { get; set; } = mp.PageName_ar;
  public string PageUrl { get; set; } = mp.PageUrl;
 }
 public class ModulePagesDto ()
 {
  public int ModuleId { get; set; }
  public int PageId { get; set; }
  public string PageName_en { get; set; } = string.Empty;
  public string PageName_ar { get; set; } = string.Empty;
  public string PageUrl { get; set; } = string.Empty;
 }
}
