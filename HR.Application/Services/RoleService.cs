using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Dtos.RoleDtos;
using HR.Application.Interfaces;
using HR.Application.Response;
using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace HR.Application.Services
{
 public class RoleService ( RoleManager<AppRole> roleManager,UserManager<AppUser> userManager,IMapper mapper,ILocalizationService localization ) : IRoleService
 {

  /// <summary>
  /// Retrieves all roles that are not marked as deleted from the database.
  /// </summary>
  /// <returns>A list of RoleDto objects representing the roles.</returns>
  public async Task<ApiResponse<IEnumerable<RoleDto>>> GetAllRolesAsync ( )
  {
   List<AppRole> roles = await roleManager.Roles.ToListAsync ();
   if(roles.Count==0)
   {
    return new ApiResponse<IEnumerable<RoleDto>>
    {
     Success=false,
     Message=localization.Get ("itemsNotFound"),
     Data=null,
     Language=localization.GetLanguage ()
    };
   }
   return new ApiResponse<IEnumerable<RoleDto>>
   {
    Success=true,
    Message=localization.Get ("itemsRetrieved"),
    Data=mapper.Map<IEnumerable<RoleDto>> (roles),
    Language=localization.GetLanguage ()
   };
  }
  /// <summary>
  /// Retrieves a role by its name from the database. If the role does not exist, it returns a RoleDto with an appropriate message.
  /// </summary>
  /// <param name="roleName">The name of the role to retrieve.</param>
  /// <returns>A RoleDto object representing the role or an error message.</returns>
  public async Task<ApiResponse<RoleDto>> GetRoleByNameAsync ( string roleName )
  {
   if (string.IsNullOrEmpty (roleName))
    return new ApiResponse<RoleDto>
    {
     Success=false,
     Message=localization.Get ("RolenameRequired"),
     Data=null,
     Language=localization.GetLanguage ()
    };
   AppRole? role = await roleManager.FindByNameAsync (roleName);
   if (role is null)
   {
    return new ApiResponse<RoleDto>
    {
     Success=false,
     Message=localization.Get ("RoleNotfound"),
     Data=null,
     Language=localization.GetLanguage ()
    };
   }
   return new ApiResponse<RoleDto>
   {
    Success=true,
    Message=localization.Get ("itemRetrieved"),
    Data=mapper.Map<RoleDto> (role),
    Language=localization.GetLanguage ()
   };
  }
  /// <summary>
  /// Creates a new role in the database. It first checks if a role with the same name already exists. If it does, it returns a RoleDto with an appropriate message. 
  /// If the creation is successful, it returns the created role as a RoleDto; 
  /// otherwise, it returns a RoleDto with error messages.
  /// </summary>
  /// <param name="Newrole"></param>
  /// <returns>
  /// A RoleDto object representing the created role or an error message.
  /// </returns>
  public async Task<ApiResponse<IEnumerable<RoleDto>>> CreateRoleAsync ( CreateRoleDto Newrole )
  {
   ApiResponse<IEnumerable<RoleDto>> response = new ()
   {
    Success=false,
    Data=null,
    Language=localization.GetLanguage ()
   };

   //if (string.IsNullOrEmpty (Newrole.Name)||string.IsNullOrEmpty (Newrole.Description))
   //{
   // response.Message=localization.Get ("missingfields");
   // return response;
   //}
   if (await roleManager.RoleExistsAsync (Newrole.Name))
   {
    response.Message=localization.Get ("RoleExists");
    return response;
   }
   AppRole identityRole = mapper.Map<AppRole> (Newrole);

   IdentityResult result = await roleManager.CreateAsync (identityRole);

   if (!result.Succeeded)
   {
    string errors = string.Empty;
    foreach (var error in result.Errors)
    {
     errors+=$"{error.Description}, ";
    }
    response.Message=errors.TrimEnd (',',' ');
    return response;
   }
   response.Success=true;
   response.Message=localization.Get ("RoleCreated");
   response.Data=mapper.Map<IEnumerable<RoleDto>> (mapper.Map<IEnumerable<RoleDto>> (await GetAllRolesAsync ()));
   return response;

  }
  /// <summary>
  /// Updates an existing role in the database. 
  /// It first checks if the role exists. 
  /// If it does, it updates the role's properties based on the provided UpdateRoleDto and the specified action (update,delete and recover).
  /// </summary>
  /// <param name="updateRole"></param>
  /// <param name="roleAction"></param>
  /// <returns>
  /// A list of RoleDto objects representing the updated roles or an error message.
  /// </returns>
  public async Task<ApiResponse<IEnumerable<RoleDto>>> UpdateRoleAsync ( UpdateRoleDto updateRole )
  {
   ApiResponse<IEnumerable<RoleDto>> response = new ()
   {
    Success=false,
    Data=null,
    Language=localization.GetLanguage ()
   };

   //if (string.IsNullOrEmpty (updateRole.NewName)||string.IsNullOrEmpty (updateRole.CurrentRoleName)||string.IsNullOrEmpty (updateRole.Description))
   //{
   // response.Message=localization.Get ("missingfields");
   // return response;
   //}

   AppRole? existingRole = await roleManager.FindByNameAsync (updateRole.CurrentRoleName);

   if (existingRole is null)
   {
    response.Message=localization.Get ("RoleNotfound");
    return response;
   }
   UpdateRoleDto updateRoleDto = new ()
   {
    NewName=(updateRole.NewName==string.Empty) ? existingRole.Name! : updateRole.NewName,
    Description=(updateRole.Description==string.Empty) ? existingRole.Description : updateRole.Description
   };
   existingRole.Name=updateRoleDto.NewName;
   existingRole.Description=updateRoleDto.Description;
   existingRole.NormalizedName=updateRole.NewName.ToUpper ();

   if (!existingRole.Name.Equals (updateRole.CurrentRoleName,StringComparison.CurrentCultureIgnoreCase))
   {
    if (await roleManager.RoleExistsAsync (updateRole.NewName))
    {
     response.Message=$"the new {localization.Get ("RoleExists")}";
     return response;
    }
   }

   IdentityResult result = await roleManager.UpdateAsync (existingRole);
   if (result.Succeeded)
   {
    string errors = string.Empty;
    foreach (var error in result.Errors)
    {
     errors+=$"{error.Description}, ";
    }
    errors=errors.TrimEnd (',',' ');

    response.Success=false;
    response.Data=null;
    response.Message=$"{localization.Get ("unexpectederror")} during updating Role which are {errors}";
    return response;
   }
   response.Success=true;
   response.Message=localization.Get ("itemsRetrieved");
   response.Data=mapper.Map<IEnumerable<RoleDto>> (await GetAllRolesAsync ());
   return response;
  }
  /// <summary>
  /// Deletes a role from the database. 
  /// It first checks if the role name is provided and then fetches the role and its associated users.
  /// </summary>
  /// <param name="roleName"></param>
  /// <returns>
  /// A tuple containing a message and a boolean indicating success.
  /// </returns>
  public async Task<ApiResponse<IEnumerable<RoleDto>>> DeleteRoleAsync ( string roleName )
  {
   ApiResponse<IEnumerable<RoleDto>> response = new ()
   {
    Success=false,
    Data=null,
    Language=localization.GetLanguage ()
   };
   if (string.IsNullOrEmpty (roleName))
   {
    response.Message=localization.Get ("RolenameRequired");
    return response;
   }

   AppRole? role = await roleManager.FindByNameAsync (roleName.ToUpper ());
   if (role is null)
   {
    response.Message=localization.Get ("RoleNotfound");
    return response;
   }
   IList<AppUser> usersInRole = await userManager.GetUsersInRoleAsync (roleName.ToUpper ());
   if (usersInRole.Count>0)
   {
    response.Message=localization.Get ("usersinrole");
    return response;
   }
   else
   {
    IdentityResult result = await roleManager.DeleteAsync (role);

    if (!result.Succeeded)
    {
     response.Success=false;
     response.Message=localization.Get ("roledeletfailed");
     response.Data=null;
     return response;
    }

    response.Success=true;
    response.Message=localization.Get ("RoleDeleted");
    response.Data=mapper.Map<IEnumerable<RoleDto>> (await GetAllRolesAsync ());
    return response;
   }
  }
  /// <summary>
  /// Retrieves a list of users assigned to a specific role. 
  /// It first checks if the role name is provided and then fetches the role and its associated users. 
  /// If the role or users are not found, appropriate error messages are returned.
  /// </summary>
  /// <param name="RoleName"></param>
  /// <returns>
  /// A list of usernames of users assigned to the specified role.
  /// </returns>
  public async Task<ApiResponse<IList<string>>> GetRoleUsersAsync ( string RoleName )
  {
   ApiResponse<IList<string>> response = new ()
   {
    Success=false,
    Data=null,
    Language=localization.GetLanguage ()
   };
   if (string.IsNullOrEmpty (RoleName))
   {
    response.Message=localization.Get ("RolenameRequired");
    return response;
   }
   AppRole? role = await roleManager.FindByNameAsync (RoleName);
   if (role is null)
   {
    response.Message=localization.Get ("RoleNotfound");
    return response;
   }
   IList<AppUser> users = await userManager.GetUsersInRoleAsync (role.Name!);

   if (users is null)
   {
    response.Message=localization.Get ("nousersrole");
    return response;
   }
   List<string> usernames = [.. users.Select (u => u.UserName!).Where (u => u!=null)];
   response.Success=true;
   response.Data=usernames;
   response.Message=localization.Get ("itemsRetrieved");
   return response;
  }
  /// <summary>
  /// Removes all users from a specific role.
  /// </summary>
  /// <param name="roleName"></param>
  /// <returns>
  /// A tuple containing a message and a boolean indicating success.
  /// </returns>
  public async Task<ApiResponse<IList<AppUser>>> RemoveUsersFromRoleAsync ( string roleName )
  {
   ApiResponse<IList<AppUser>> response = new ()
   {
    Success=false,
    Data=null,
    Language=localization.GetLanguage ()
   };
   if (string.IsNullOrEmpty (roleName))
   {
    response.Message=localization.Get ("RolenameRequired");
    return response;
   }
   AppRole? role = await roleManager.FindByNameAsync (roleName);
   if (role is null)
   {
    response.Message=localization.Get ("RoleNotfound");
    return response;
   }
   IList<AppUser> usersInRole = await userManager.GetUsersInRoleAsync (role.Name!);
   if (usersInRole.Count==0)
   {
    response.Message=localization.Get ("nousersinrole");
    return response;
   }
   foreach (var user in usersInRole)
   {
    IdentityResult result = await userManager.RemoveFromRoleAsync (user,role.Name!);
    if (!result.Succeeded)
    {
     response.Message=$"{localization.Get ("deleteallusersfromrolefailed")}";
     return response;
    }
   }
   response.Success=true;
   response.Message=$"{localization.Get ("deleteallusersfromrole")}";
   response.Data=await userManager.GetUsersInRoleAsync (role.Name!);
   return response;
  }

 }
}