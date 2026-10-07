namespace HR.Domain.Models.Authorization
{
 public class Module_SubModules
 {
  public int ModuleId { get; set; }
  public int SubModuleId { get; set; }

  public virtual Module? Module { get; set; }
  public virtual SubModules? SubModule { get; set; }


 }
}
