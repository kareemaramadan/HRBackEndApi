namespace HR.Domain.Models.Authorization
{
 public class SubModulePages
 {
  public int PageId { get; set; }
  public int SubModuleId { get; set; }
  public string PageName_en { get; set; } = string.Empty;
  public string PageName_ar { get; set; } = string.Empty;
  public string PageUrl { get; set; } = string.Empty;


  public virtual SubModules? SubModule { get; set; }
  public virtual ICollection<Role_Page_Permissions>? Role_Page_Permissions { get; set; }
  public virtual ICollection<User_Page_Permission>? User_Page_Permissions { get; set; }
  public virtual ICollection<Role_SubModulePages>? Role_SubModulePages { get; set; }

 }
}
