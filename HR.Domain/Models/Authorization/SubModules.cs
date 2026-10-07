namespace HR.Domain.Models.Authorization
{
 public class SubModules
 {
  public int SubModuleId { get; set; }
  public string SubModuleName_en { get; set; }
  public string SubModuleName_ar { get; set; }


  public virtual ICollection<Module_SubModules>? Module_SubModules { get; set; }
  public virtual ICollection<SubModulePages>? SubModulePages { get; set; }

 }
}
