using HR.Domain.Models.Authorization;

namespace HR.Application.Dtos.AuthDtos
{
 public class SubModulePagesDtowithModule ( SubModulePages mp,string language )
 {
  public int SubModuleId { get; set; } = mp.SubModuleId;
  public string SubModuleName { get; set; } = language=="en" ? mp.SubModule!.SubModuleName_en : mp.SubModule!.SubModuleName_ar;
  public int? PageId { get; set; } = mp.PageId;
  public string PageName_en { get; set; } = mp.PageName_en;
  public string PageName_ar { get; set; } = mp.PageName_ar;
  public string PageUrl { get; set; } = mp.PageUrl;
 }
 public class SubModulePagesDto ( )
 {
  public int SubModuleId { get; set; }
  public int PageId { get; set; }
  public string PageName_en { get; set; } = string.Empty;
  public string PageName_ar { get; set; } = string.Empty;
  public string PageUrl { get; set; } = string.Empty;
 }
}
