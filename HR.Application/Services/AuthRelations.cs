using HR.Application.Interfaces;
using HR.Application.Response;
using HR.Domain.Models.Authorization;
using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace HR.Application.Services
{
 public class AuthRelations (
  ILocalizationService localization,
  IBaseService<Role_SubModulePages> rolePagesService,
  UserManager<AppUser> userManager,
  RoleManager<AppRole> rolemanager,
  IBaseService<User_Page_Permissions> userpageService,
  IBaseService<Role_Page_Permissions> rolepageservice,
  IBaseService<Permission> permissionservice ) : IAuthRelations
 {

  public async Task<ApiResponse<string?>> Addpagestorole ( string username )
  {
   ApiResponse<string?> errorsresponse = new ()
   {
    Success=false,
    Data=null,
    Language=localization.GetLanguage ()
   };
   AppUser? user = await userManager.FindByNameAsync (username);
   if (user is null)
   {
    errorsresponse.Message=localization.Get ("missingfields");
    return errorsresponse;
   }
   else
   {
    List<string> userroles = (List<string>)await userManager.GetRolesAsync (user);
    if (userroles.Count>0)
    {
     foreach (string rolename in userroles)
     {
      var AppRole = await rolemanager.FindByNameAsync (rolename);
      var pages = rolePagesService.GetAllAsync (r => r.RoleId==AppRole!.Id);
      if (pages is null)
      {
       errorsresponse.Message=localization.Get ("modulepagesNotfound");
       return errorsresponse;
      }
      foreach (var page in pages.Result.Data!)
      {
       int pageId = page.PageId;
       string roleId = AppRole!.Id;

       var defaultpermission = permissionservice.FindItemAsync (p => p.PermissionName=="FullControl");
       if (!defaultpermission.Result.Success)
       {
        errorsresponse.Message=localization.Get ("PermissionNotFound");
        return errorsresponse;
       }
       Role_Page_Permissions RPP = new ()
       {
        RoleId=roleId,
        PageId=pageId,
        IsAllowed=true,
        PermissionId=defaultpermission.Result.Data!.PermissionId
       };

       var checkrolepage = rolepageservice.IsExistAsync (rp => rp.RoleId==AppRole!.Id&&rp.PageId==pageId);
       if (!checkrolepage.Result.Data)
       {
        var addpagestorole = rolepageservice.CreateAsync (RPP);
        var checkuserpage = userpageService.IsExistAsync (up => up.UserId==user.Id&&up.PageId==pageId);
        if (!checkuserpage.Result.Data)
        {
         User_Page_Permissions UPP = new ()
         {
          UserId=user.Id,
          PageId=pageId,
          IsAllowed=true,
          PermissionId=defaultpermission.Result.Data!.PermissionId
         };
         var adduserpages = userpageService.CreateAsync (UPP);
        }

       }
       else
       {

       }
      }
     }
     errorsresponse.Message=localization.Get ("itemcreated");
     return errorsresponse;
    }
    else
    {
     errorsresponse.Message=localization.Get ("nousersrole");
     return errorsresponse;
    }
   }
  }
 }
}
