using HR.Application.Interfaces;
using HR.Application.Response;
using HR.Domain.Models.Authorization;
using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace HR.Application.Services
{
 public class AuthRelations (
  ILocalizationService localization,
  IBaseService<Role_SubModules> rolesubmodulesService,
  UserManager<AppUser> userManager,
  RoleManager<AppRole> rolemanager,
  IBaseService<User_Page_Permissions> userpagepermissionService,
  IBaseService<Role_Page_Permissions> rolepagepermissionservice,
  IBaseService<SubModulePages> submodulepagesservice,
  IBaseService<Permission> permissionservice ) : IAuthRelations
 {
  public async Task<ApiResponse<string?>> AddUserToRole ( string username,string roleId )
  {
   ApiResponse<string?> response = new ()
   {
    Success=false,
    Data=null,
    Language=localization.GetLanguage ()
   };
   AppUser? user = await userManager.FindByNameAsync (username);
   AppRole? role = await rolemanager.FindByIdAsync (roleId);

   var getrolepagespermission = await rolepagepermissionservice.GetAllAsync (rpp => rpp.RoleId==roleId);

   var permissionPagesforRole = getrolepagespermission.Data!;
   foreach (var rolepermission in permissionPagesforRole)
   {
    var rolepageId = rolepermission.PageId;
    var rolepermissionId = rolepermission.PermissionId;
    User_Page_Permissions user_Page_Permissions = new ()
    {
     PermissionId=rolepermissionId,
     PageId=rolepageId,
     UserId=user!.Id,
     IsAllowed=true
    };

    await userpagepermissionService.CreateAsync (user_Page_Permissions);
   }
   response.Success=true;
   response.Message=localization.Get ("RoleAddedSuccessfully");
   return response;

  }
  public async Task<ApiResponse<string?>> RemoveUserFromRole ( string username,string roleId )
  {
   ApiResponse<string?> response = new ()
   {
    Success=false,
    Data=null,
    Language=localization.GetLanguage ()
   };
   AppUser? user = await userManager.FindByNameAsync (username);
   AppRole? role = await rolemanager.FindByIdAsync (roleId);

   var getrolepagespermission = await rolepagepermissionservice.GetAllAsync (rpp => rpp.RoleId==roleId);

   var permissionPagesforRole = getrolepagespermission.Data!;


   foreach (var permission in permissionPagesforRole)
   {
    var rolepageId = permission.PageId;
    await userpagepermissionService.DeleteAsync (upp => upp.UserId==user!.Id&&upp.PageId==rolepageId);
   }

   response.Success=true;
   response.Message=localization.Get ("RoleAddFailed");
   return response;
  }


  public async Task<ApiResponse<string?>> AddSubModuleToRole ( string roleId,int ModuleId,int SubModuleId )
  {
   ApiResponse<string?> response = new ()
   {
    Success=false,
    Data=null,
    Language=localization.GetLanguage ()
   };
   var getAllpagesforSubModule = await submodulepagesservice.GetAllAsync (p => p.SubModuleId==SubModuleId);

   AppRole? role = await rolemanager.FindByIdAsync (roleId);
   var getAllUsersforRole = await userManager.GetUsersInRoleAsync (role!.Name!);

   var getpermission = await permissionservice.FindItemAsync (p => p.PermissionName=="FullControl");
   int permId = getpermission.Data!.PermissionId;

   Role_SubModules role_Sub = new ()
   {
    RoleId=role.Id,
    SubModuleId=SubModuleId,
    ModuleId=ModuleId
   };
   await rolesubmodulesService.CreateAsync (role_Sub);

   if (getAllpagesforSubModule.Data!.Any ())
   {
    foreach (var page in getAllpagesforSubModule.Data!)
    {

     //add pageId with RoleId and PermissionID to RolePagePermissions Table
     //=====================================================================

     var isexists = await rolepagepermissionservice.IsExistAsync (rpp => rpp.PageId==page.PageId&&rpp.RoleId==role.Id);

     if (!isexists.Success)
     {
      Role_Page_Permissions role_Page = new ()
      {
       PageId=page.PageId,
       RoleId=role.Id,
       PermissionId=permId,
       IsAllowed=true
      };
      await rolepagepermissionservice.CreateAsync (role_Page);
      //add PageId for AllUsers in Role with PermissionID to UserPagePermissions Table

      foreach (var user in getAllUsersforRole)
      {
       User_Page_Permissions user_Page = new ()
       {
        UserId=user.Id,
        PageId=page.PageId,
        PermissionId=permId,
        IsAllowed=true
       };
       await userpagepermissionService.CreateAsync (user_Page);
      }
     }
    }
    response.Success=true;
    response.Message=localization.Get ("itemapply");
    return response;
   }
   else
   {
    //error this submodulehas no pages assigned
    response.Message=localization.Get ("NoPagesfound");
    return response;
   }
  }
  public async Task<ApiResponse<string?>> RemoveSubModuleFromRole ( string roleName,int ModuleId,int SubModuleId )
  {
   ApiResponse<string?> response = new ()
   {
    Success=false,
    Data=null,
    Language=localization.GetLanguage ()
   };
   var getAllpagesforSubModule = await submodulepagesservice.GetAllAsync (p => p.SubModuleId==SubModuleId);

   AppRole? role = await rolemanager.FindByNameAsync (roleName);
   var getAllUsersforRole = await userManager.GetUsersInRoleAsync (role!.Name!);

   //var getpermission = await permissionservice.FindItemAsync (p => p.PermissionName=="FullControl");
   //int permId = getpermission.Data!.PermissionId;

   if (getAllpagesforSubModule.Data!.Any ())
   {
    foreach (var page in getAllpagesforSubModule.Data!)
    {

     //add pageId with RoleId and PermissionID to RolePagePermissions Table
     //=====================================================================
     Role_Page_Permissions role_Page = new ()
     {
      PageId=page.PageId,
      RoleId=role.Id
     };
     await rolepagepermissionservice.DeleteAsync (role_Page);
     //add PageId for AllUsers in Role with PermissionID to UserPagePermissions Table

     foreach (var user in getAllUsersforRole)
     {
      User_Page_Permissions user_Page = new ()
      {
       UserId=user.Id,
       PageId=page.PageId
      };
      await userpagepermissionService.DeleteAsync (user_Page);

      Role_SubModules role_Sub = new ()
      {
       RoleId=role.Id,
       SubModuleId=SubModuleId,
       ModuleId=ModuleId
      };
      await rolesubmodulesService.DeleteAsync (role_Sub);
     }
    }
    response.Success=true;
    response.Message=localization.Get ("itemsdeleted");
    return response;
   }
   else
   {
    //error this submodulehas no pages assigned
    response.Message=localization.Get ("NoPagesfound");
    return response;
   }
  }

  public async Task<ApiResponse<string>> AddPageToSubModule ( int SubmoduleId,int PageId )
  {
   var getrolesforsubmodule = await rolesubmodulesService.GetAllAsync (RP => RP.SubModuleId==SubmoduleId);

   var getpermission = await permissionservice.FindItemAsync (p => p.PermissionName=="FullControl");
   int permId = getpermission.Data!.PermissionId;

   if (getrolesforsubmodule!.Data!.Any ())
   {
    foreach (var role in getrolesforsubmodule.Data!)
    {
     Role_Page_Permissions role_Page = new ()
     {
      RoleId=role.RoleId,
      PageId=PageId,
      IsAllowed=true,
      PermissionId=permId
     };
     await rolepagepermissionservice.CreateAsync (role_Page);

     var getrolessubmodule = await rolesubmodulesService.GetAllAsync (sb => sb.SubModuleId==SubmoduleId);

     foreach (var rolemodule in getrolessubmodule.Data!)
     {
      var roleId = rolemodule.RoleId;
      var moduleId = rolemodule.ModuleId;

      var add = await AddSubModuleToRole (roleId,moduleId,SubmoduleId);

     }
     ;
    }
    return new ApiResponse<string>
    {
     Success=true,
     Data=null,
     Message=localization.Get ("itemapply"),
     Language=localization.GetLanguage ()
    };

   }
   return new ApiResponse<string>
   {
    Success=true,
    Data=null,
    Message=localization.Get ("itemapplyfailed"),
    Language=localization.GetLanguage ()
   };

  }

  public async Task<ApiResponse<string>> RemovePageFromSubModule ( int SubmoduleId,int PageId )
  {
   var getrolesforsubmodule = await rolesubmodulesService.GetAllAsync (RP => RP.SubModuleId==SubmoduleId);

   if (getrolesforsubmodule!.Data!.Any ())
   {
    foreach (var role in getrolesforsubmodule.Data!)
    {
     Role_Page_Permissions role_Page = new ()
     {
      RoleId=role.RoleId,
      PageId=PageId
     };
     await rolepagepermissionservice.DeleteAsync (role_Page);

     var getrolessubmodule = await rolesubmodulesService.GetAllAsync (sb => sb.SubModuleId==SubmoduleId);

     foreach (var rolemodule in getrolessubmodule.Data!)
     {
      var roleId = rolemodule.RoleId;
      var moduleId = rolemodule.ModuleId;

      await RemoveSubModuleFromRole (roleId,moduleId,SubmoduleId);

     }
    }
    return new ApiResponse<string>
    {
     Success=true,
     Data=null,
     Message=localization.Get ("itemsdeleted"),
     Language=localization.GetLanguage ()
    };
   }
   return new ApiResponse<string>
   {
    Success=true,
    Data=null,
    Message=localization.Get ("itemapplyfailed"),
    Language=localization.GetLanguage ()
   };

  }
 }
}
