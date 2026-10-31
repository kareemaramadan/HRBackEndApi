using HR.Application.Dtos.AppDtos;
using HR.Application.Interfaces;
using HR.Application.Response;
using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRBackEndApi.Controllers.AuthControllers
{
 /// <summary>
 /// Role Management API
 /// </summary>
 /// <remarks>
 /// Manages system roles including creation, retrieval, updating, and deletion.
 /// 
 /// **Base Route:** `/api/Role`
 /// **Authentication:** Required (Bearer token)
 /// **Authorization:** Typically requires Admin role
 /// **Content Type:** application/json
 /// 
 /// **Role Hierarchy:**
 /// Roles are security containers that group permissions and can be assigned to users.
 /// Each role can have multiple permissions and users can be assigned multiple roles.
 /// 
 /// **Common Roles:**
 /// - Admin: Full system access
 /// - Manager: Department/team management
 /// - User: Standard user access
 /// - Guest: Limited read-only access
 /// </remarks>
 [Authorize]
 [ApiController]
 [Route ("api/[controller]")]
 [Produces ("application/json")]

 public class RoleController ( IRoleService roleService,ILocalizationService localization ) : ControllerBase
 {
  /// <summary>
  /// Retrieve all roles available in the system
  /// </summary>
  /// <remarks>
  /// Returns a complete list of all roles with their identifiers and descriptions.
  /// 
  /// **Authorization:** Requires authenticated user (typically Admin or Manager role)
  /// 
  /// **Response Format:**
  /// Each role includes:
  /// - RoleId: Unique role identifier
  /// - Name: Role display name (e.g., "Admin", "Manager", "User")
  /// - Description: Purpose and permissions summary
  /// 
  /// **Use Cases:**
  /// - Display roles in an admin UI for role assignment
  /// - Get list of available roles for filtering/searching
  /// - Populate role dropdown in user management forms
  /// 
  /// **Performance Note:** Results are ordered by RoleId
  /// </remarks>
  /// <example>
  /// Request:
  /// GET /api/Role/GetAllRoles
  /// Header: Authorization: Bearer {valid_jwt_token}
  /// 
  /// Success Response (200 OK):
  /// {
  ///   "success": true,
  ///   "data": [
  ///     {
  ///       "roleId": "1",
  ///       "name": "Admin",
  ///       "description": "Administrator with full system access and all permissions"
  ///     },
  ///     {
  ///       "roleId": "2",
  ///       "name": "Manager",
  ///       "description": "Manager can oversee team and approve requests"
  ///     },
  ///     {
  ///       "roleId": "3",
  ///       "name": "User",
  ///       "description": "Standard user with basic read and limited write permissions"
  ///     }
  ///   ],
  ///   "message": ""
  /// }
  /// 
  /// Database Error (400):
  /// {
  ///   "success": false,
  ///   "message": "Failed to retrieve roles from database",
  ///   "data": null
  /// }
  /// </example>
  /// <returns>ApiResponse containing collection of RoleDto objects</returns>
  /// <response code="200">Roles retrieved successfully</response>
  /// <response code="400">Database error or retrieval failed</response>
  /// <response code="401">Unauthorized - authentication token required or invalid</response>
  [HttpGet ("GetAllRoles")]
  [ProducesResponseType (typeof (ApiResponse<IEnumerable<RoleDto>>),StatusCodes.Status200OK)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status400BadRequest)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status401Unauthorized)]
  public async Task<ActionResult<ApiResponse<IEnumerable<RoleDto>>>> GetAllRolesAsync ( )
  {
   ApiResponse<IEnumerable<RoleDto>> roles = await roleService.GetAllRolesAsync ();
   return (roles.Success) ? Ok (roles) : BadRequest (roles);
  }
  /// <summary>
  /// Retrieve a specific role by name
  /// </summary>
  /// <remarks>
  /// Finds and returns a single role matching the provided name.
  /// 
  /// **Authorization:** Requires authenticated user
  /// 
  /// **Search Behavior:**
  /// - Search is case-insensitive by default
  /// - Returns exactly one role or not found
  /// - Useful for validation and role lookups
  /// 
  /// **Error Scenarios:**
  /// - 400 Bad Request: Role with name does not exist
  /// - 401 Unauthorized: Invalid or missing authentication token
  /// </remarks>
  /// <example>
  /// Request:
  /// GET /api/Role/GetRoleByName?roleName=Admin
  /// Header: Authorization: Bearer {token}
  /// 
  /// Success Response (200):
  /// {
  ///   "success": true,
  ///   "data": {
  ///     "roleId": "1",
  ///     "name": "Admin",
  ///     "description": "Administrator with full system access"
  ///   },
  ///   "message": ""
  /// }
  /// 
  /// Not Found Response (400):
  /// {
  ///   "success": false,
  ///   "message": "role not found",
  ///   "data": null
  /// }
  /// </example>
  /// <param name="roleName">
  /// The role name to search for.
  /// Case-insensitive, must be exact match.
  /// Example: "Admin", "Manager", "User"
  /// </param>
  /// <returns>ApiResponse with RoleDto when found, null data if not found</returns>
  /// <response code="200">Role found and returned</response>
  /// <response code="400">Role not found with this name</response>
  /// <response code="401">Unauthorized - authentication required</response>
  [HttpGet ("GetRoleByName")]
  [ProducesResponseType (typeof (ApiResponse<RoleDto>),StatusCodes.Status200OK)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status400BadRequest)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status401Unauthorized)]
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
