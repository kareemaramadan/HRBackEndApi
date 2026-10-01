using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Dtos.RoleDtos;
using HR.Application.Interfaces;
using HR.Application.Response;
using HR.Application.Services;
using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRBackEndApi.Controllers.AuthControllers
{

 [ApiController]
 [Route ("api/[controller]")]

 public class RoleController ( IRoleService roleService,ILocalizationService localization ) : ControllerBase
 {
  /// <summary>
  /// Gets a list of all roles in the system
  /// </summary>
  /// <returns>
  /// A list of all roles in the system
  /// </returns>
  [HttpGet ("GetAllRoles")]
  public async Task<ActionResult<ApiResponse<IEnumerable<RoleDto>>>> GetAllRolesAsync ( )
  {
   ApiResponse<IEnumerable<RoleDto>> roles = await roleService.GetAllRolesAsync ();
   return (roles.Success) ? Ok (roles) : BadRequest (roles);
  }
  /// <summary>
  /// Gets a specific role by its name
  /// </summary>
  /// <param name="roleName"></param>
  /// <returns>
  /// The role with the specified name
  /// </returns>
  [HttpGet ("GetRoleByName")]
  public async Task<ActionResult<ApiResponse<RoleDto>>> GetRoleByNameAsync (string roleName )
  {
   ApiResponse<RoleDto> role = await roleService.GetRoleByNameAsync (roleName);
   return (role.Success) ? Ok (role) : BadRequest (role);
  }
  /// <summary>
  /// / Gets a list of users assigned to a specific role
  /// </summary>
  /// <param name="roleName"></param>
  /// <returns>
  /// A list of usernames assigned to the specified role
  /// </returns>
  [HttpGet ("GetRoleUsers/{roleName}")]
  public async Task<ActionResult<ApiResponse<IList<string>>>> GetRoleUsers ( string roleName )
  {
   ApiResponse<IList<string>> result = await roleService.GetRoleUsersAsync (roleName);
   return (result.Success) ? Ok (result) : BadRequest (result);
  }
  /// <summary>
  /// Creates a new role in the system
  /// </summary>
  /// <param name="role"></param>
  /// <returns>
  /// The created role
  /// </returns>
  [HttpPost ("AddNewRole")]
  public async Task<ActionResult<ApiResponse<IEnumerable<RoleDto>>>> CreateRoleAsync ( [FromBody] CreateRoleDto role )
  {
   if (!ModelState.IsValid)
    return BadRequest (new ApiResponse<IEnumerable<RoleDto>> ()
    {
     Success=false,
     Data=null,
     Message=localization.Get ("invaliddata"),
     Language=localization.GetLanguage ()
    });
   ApiResponse<IEnumerable<RoleDto>> createdRole = await roleService.CreateRoleAsync (role);
   return (createdRole.Success) ? Created ("",createdRole) : BadRequest (createdRole);
  }
  /// <summary>
  /// Updates an existing role in the system
  /// </summary>
  /// <param name="role"></param>
  /// <returns>
  /// The updated role
  /// </returns>
  [HttpPut]
  [Route ("UpdateRole")]
  public async Task<ActionResult<ApiResponse<IEnumerable<RoleDto>>>> UpdateRoleAsync ( [FromBody] UpdateRoleDto role )
  {
   if (!ModelState.IsValid)
    return BadRequest (new ApiResponse<IEnumerable<RoleDto>> ()
    {
     Success=false,
     Data=null,
     Message=localization.Get ("invaliddata"),
     Language=localization.GetLanguage ()
    });
   ApiResponse<IEnumerable<RoleDto>> updatedRoles = await roleService.UpdateRoleAsync (role);
   return (updatedRoles.Success) ? Ok (updatedRoles) : BadRequest (updatedRoles);
  }
  /// <summary>
  /// Deletes a role from the system
  /// </summary>
  /// <param name="roleName"></param>
  /// <returns>
  /// The deleted role
  /// </returns>
  [HttpDelete]
  [Route ("DeleteRole/{rolename}")]
  public async Task<ActionResult<ApiResponse<IEnumerable<RoleDto>>>> DeleteRoleAsync ( string roleName )
  {
   ApiResponse<IEnumerable<RoleDto>> deletedRole = await roleService.DeleteRoleAsync (roleName);
   return (deletedRole.Success) ? Ok (deletedRole) : BadRequest (deletedRole);
  }
  /// <summary>
  /// Removes all users from a specific role
  /// </summary>
  /// <param name="roleName"></param>
  /// <returns>
  /// A message indicating the success or failure of the operation
  /// </returns>
  [HttpDelete]
  [Route ("RemoveAllUsersFromRole/{rolename}")]
  public async Task<ActionResult<ApiResponse<IList<AppUser>>>> RemoveAllUsersFromRoleAsync ( string roleName )
  {
   ApiResponse<IList<AppUser>> removedusers = await roleService.RemoveUsersFromRoleAsync (roleName);
   return (removedusers.Success) ? Ok (removedusers) : BadRequest (removedusers);
  }
 }
}
