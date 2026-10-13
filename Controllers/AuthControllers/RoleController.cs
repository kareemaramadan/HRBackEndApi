using HR.Application.Dtos.AppDtos;
using HR.Application.Interfaces;
using HR.Application.Response;
using HR.Domain.Models.Identity;
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
  /// <remarks>
  /// Returns available roles. Results are returned with role id, name and description.
  /// </remarks>
  /// <example>
  /// GET /api/Role/GetAllRoles
  /// // Success (200)
  /// { "success": true, "data": [ { "RoleId":"1","Name":"Admin","Description":"" } ], "message": "" }
  /// </example>
  /// <returns>ApiResponse with the collection of roles.</returns>
  [HttpGet ("GetAllRoles")]
  public async Task<ActionResult<ApiResponse<IEnumerable<RoleDto>>>> GetAllRolesAsync ( )
  {
   ApiResponse<IEnumerable<RoleDto>> roles = await roleService.GetAllRolesAsync ();
   return (roles.Success) ? Ok (roles) : BadRequest (roles);
  }
  /// <summary>
  /// Gets a specific role by its name
  /// </summary>
  /// <remarks>
  /// Returns the role DTO matching the provided role name. If not found a BadRequest is returned.
  /// </remarks>
  /// <example>
  /// GET /api/Role/GetRoleByName?roleName=Admin
  /// // Success (200)
  /// { "success": true, "data": { "RoleId":"1","Name":"Admin" }, "message": "" }
  /// // Error (400)
  /// { "success": false, "message": "role not found" }
  /// </example>
  /// <param name="roleName">Role name to lookup.</param>
  /// <returns>ApiResponse with the RoleDto when found.</returns>
  [HttpGet ("GetRoleByName")]
  public async Task<ActionResult<ApiResponse<RoleDto>>> GetRoleByNameAsync ( string roleName )
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
  /// <remarks>
  /// Creates a new role. Returns Created (201) with the current roles list if successful. Validation errors return BadRequest.
  /// </remarks>
  /// <example>
  /// POST /api/Role/AddNewRole
  /// { "Name": "Editor", "Description": "Can edit content" }
  /// // Success (201)
  /// { "success": true, "data": [ { "RoleId":"2","Name":"Editor" } ], "message": "" }
  /// // Validation error (400)
  /// { "success": false, "message": "invaliddata" }
  /// </example>
  /// <param name="role">CreateRoleDto with role name and description.</param>
  /// <returns>ApiResponse with the created role(s).</returns>
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
  /// Updates an existing role in the system,with RoleId,RoleName and Role description.
  /// </summary>
  /// <remarks>
  /// Updates the specified role by id. Returns BadRequest on validation error or if update fails.
  /// </remarks>
  /// <example>
  /// PUT /api/Role/UpdateRole/2
  /// { "Name": "Editor", "Description": "Updated" }
  /// // Success (200)
  /// { "success": true, "data": [ { "RoleId":"2","Name":"Editor" } ], "message": "" }
  /// </example>
  /// <param name="id">Role identifier to update.</param>
  /// <param name="role">CreateRoleDto containing new values.</param>
  /// <returns>ApiResponse with the updated roles collection.</returns>
  [HttpPut]
  [Route ("UpdateRole/{id}")]
  public async Task<ActionResult<ApiResponse<IEnumerable<RoleDto>>>> UpdateRoleAsync ( string id,[FromBody] CreateRoleDto role )
  {
   if (!ModelState.IsValid||string.IsNullOrEmpty (id))
    return BadRequest (new ApiResponse<IEnumerable<RoleDto>> ()
    {
     Success=false,
     Data=null,
     Message=localization.Get ("invaliddata"),
     Language=localization.GetLanguage ()
    });
   ApiResponse<IEnumerable<RoleDto>> updatedRoles = await roleService.UpdateRoleAsync (id,role);
   return (updatedRoles.Success) ? Ok (updatedRoles) : BadRequest (updatedRoles);
  }
  /// <summary>
  /// Deletes a role from the system
  /// </summary>
  /// <remarks>
  /// Deletes the role with the specified name. Returns updated roles list on success or BadRequest on failure.
  /// </remarks>
  /// <example>
  /// DELETE /api/Role/DeleteRole/Admin
  /// // Success (200)
  /// { "success": true, "data": [ ], "message": "Role deleted" }
  /// </example>
  /// <param name="roleName">Name of the role to delete.</param>
  /// <returns>ApiResponse with remaining roles.</returns>
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
  /// <remarks>
  /// Removes every user currently assigned to the specified role and returns the removed user list. Returns BadRequest if the role does not exist.
  /// </remarks>
  /// <example>
  /// DELETE /api/Role/RemoveAllUsersFromRole/Admin
  /// // Success (200)
  /// { "success": true, "data": [ { "UserName":"jdoe" } ], "message": "" }
  /// </example>
  /// <param name="roleName">Role name to remove users from.</param>
  /// <returns>ApiResponse with removed users.</returns>
  [HttpDelete]
  [Route ("RemoveAllUsersFromRole/{rolename}")]
  public async Task<ActionResult<ApiResponse<IList<AppUser>>>> RemoveAllUsersFromRoleAsync ( string roleName )
  {
   ApiResponse<IList<AppUser>> removedusers = await roleService.RemoveUsersFromRoleAsync (roleName);
   return (removedusers.Success) ? Ok (removedusers) : BadRequest (removedusers);
  }
 }
}
