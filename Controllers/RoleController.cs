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
    [Route("api/[controller]")]
   
    public class RoleController( IRoleService roleService) : ControllerBase
    {
        [HttpPost("AddNewRole")]
        public async Task<ActionResult<RoleDto>> createRoleAsync ( [FromBody] CreateRoleDto role )
        {
            if ( !ModelState.IsValid )
                return BadRequest ( ModelState );

            var createdRole = await roleService.CreateRoleAsync ( role );

            if( createdRole.Message is not null )
                return BadRequest( createdRole.Message );

            return Created ("", createdRole );
        }

        [HttpGet ( "GetAllRoles" )]
        public async Task<ActionResult<IEnumerable<RoleDto>>> getAllRolesAsync()
        {
            var roles = await roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet ( "GetRoleByName" )]
        public async Task<ActionResult<RoleDto>> getRoleByNameAsync (string roleName )
        {
            var role = await roleService.GetRoleByNameAsync ( roleName );
            return Ok ( role );
        }

        [HttpPut]
        [Route ( "UpdateRole/{roleAction}" )]
        public async Task<ActionResult<IEnumerable<RoleDto>>> updateRoleAsync ( [FromBody] UpdateRoleDto role,string roleAction )
        {
            if ( !ModelState.IsValid )
                return BadRequest ( ModelState );
            IEnumerable<RoleDto> updatedRoles = await roleService.UpdateRoleAsync ( role, roleAction );
            if ( updatedRoles == null || !updatedRoles.Any ( ) )
                return NotFound ( "No roles were found matching the update criteria." );
            return Ok ( updatedRoles );
        }

        [HttpDelete]
        [Route ( "DeleteRole/{roleName}" )]
        public async Task<ActionResult<IEnumerable<RoleDto>>> deleteRoleAsync ([FromBody] string roleName )
        {
            IEnumerable<RoleDto> deletedRole = await roleService.DeleteRoleAsync ( roleName );
            if ( deletedRole == null || !deletedRole.Any ( ) )
                return NotFound ( "No roles were found matching the delete criteria." );
            return Ok ( deletedRole );
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
            RoleUsersDto result = await roleService.GetRoleUsersAsync ( roleName );
            return Ok ( result );
        }

    }
}
