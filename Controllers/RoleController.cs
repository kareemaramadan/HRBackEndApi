using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Dtos.RoleDtos;
using HR.Application.Interfaces;
using HR.Application.Services;
using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRBackEndApi.Controllers
{

    [ApiController]
    [Route ( "api/[controller]" )]

    public class RoleController ( IRoleService roleService ) : ControllerBase
    {
        /// <summary>
        /// Gets a list of all roles in the system
        /// </summary>
        /// <returns>
        /// A list of all roles in the system
        /// </returns>
        [HttpGet ( "GetAllRoles" )]
        public async Task<ActionResult<IEnumerable<RoleDto>>> getAllRolesAsync ( )
        {
            var roles = await roleService.GetAllRolesAsync ( );
            return Ok ( roles );
        }

        /// <summary>
        /// Gets a specific role by its name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>
        /// The role with the specified name
        /// </returns>
        [HttpGet ( "GetRoleByName" )]
        public async Task<ActionResult<RoleDto>> getRoleByNameAsync ( string roleName )
        {
            var role = await roleService.GetRoleByNameAsync ( roleName );
            return Ok ( role );
        }

        /// <summary>
        /// / Gets a list of users assigned to a specific role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>
        /// A list of usernames assigned to the specified role
        /// </returns>
        [HttpGet ( "GetRoleUsers/{roleName}" )]
        public async Task<ActionResult<IList<string>>> GetRoleUsers ( string roleName )
        {
            if ( string.IsNullOrEmpty ( roleName ) )
            {
                return BadRequest ( "Role name is required" );
            }
            IList<string> result = await roleService.GetRoleUsersAsync ( roleName );
            if ( result.Count == 0 )
                return BadRequest ( "No users found for the specified role." );
            return Ok ( result );
        }

        /// <summary>
        /// Creates a new role in the system
        /// </summary>
        /// <param name="role"></param>
        /// <returns>
        /// The created role
        /// </returns>
        [HttpPost ( "AddNewRole" )]
        public async Task<ActionResult> createRoleAsync ( [FromBody] CreateRoleDto role )
        {
            if ( !ModelState.IsValid )
                return BadRequest ( ModelState );

            var (createdRole, isSuccess) = await roleService.CreateRoleAsync ( role );

            if ( !isSuccess )
                return BadRequest ( createdRole.Message );

            return Created ("",createdRole);
        }

        /// <summary>
        /// Updates an existing role in the system
        /// </summary>
        /// <param name="role"></param>
        /// <param name="roleAction"></param>
        /// <returns>
        /// The updated role
        /// </returns>
        [HttpPut]
        [Route ( "UpdateRole" )]
        public async Task<ActionResult<IEnumerable<RoleDto>>> updateRoleAsync ( [FromBody] UpdateRoleDto role)
        {
            if ( !ModelState.IsValid )
                return BadRequest ( ModelState );
            IEnumerable<RoleDto> updatedRoles = await roleService.UpdateRoleAsync ( role);
            if ( updatedRoles == null || !updatedRoles.Any ( ) )
                return NotFound ( "No roles were found matching the update criteria." );
            return Ok ( updatedRoles );
        }
       
        /// <summary>
        /// Deletes a role from the system
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>
        /// The deleted role
        /// </returns>
        [HttpDelete]
        [Route ( "DeleteRole" )]
        public async Task<ActionResult> deleteRoleAsync ([FromBody]string roleName )
        {
            var (message, isSuccess) = await roleService.DeleteRoleAsync ( roleName );
            if ( !isSuccess )
                return  BadRequest(message);
            return Ok ( message);
        }

        /// <summary>
        /// Removes all users from a specific role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>
        /// A message indicating the success or failure of the operation
        /// </returns>
        [HttpDelete]
        [Route ( "RemoveUsersFromRole" )]
        public async Task<ActionResult> removeUsersFromRoleAsync ( [FromBody] string roleName )
        {
            var (message, isSuccess) = await roleService.RemoveUsersFromRoleAsync ( roleName );
            if ( !isSuccess )
                return BadRequest ( message );
            return Ok ( message );
        }

    }
}
