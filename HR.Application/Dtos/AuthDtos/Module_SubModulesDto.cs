using HR.Domain.Models.Authorization;
using System.ComponentModel.DataAnnotations;

namespace HR.Application.Dtos.AuthDtos
{
 public class Module_SubModulesDto ( Module_SubModules msm,string language )
 {
  public int ModuleId { get; set; } = msm.ModuleId;
  public string ModuleName { get; set; } = language=="ar" ? msm.Module!.ModuleName_ar : msm.Module!.ModuleName_en;
  public int SubModuleId { get; set; } = msm.SubModuleId;
  public string SubModuleName { get; set; } = language=="ar" ? msm.SubModule!.SubModuleName_ar : msm.SubModule!.SubModuleName_en;

 }
    public class Module_SubModuleDto()
    {
        [Required]
        public int ModuleId { get; set; }
        [Required]
        public int SubModuleId { get; set; }
    }



}
