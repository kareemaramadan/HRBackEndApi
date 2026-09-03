using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Dtos.RoleDtos;
using HR.Application.Interfaces;
using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRBackEndApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize (Roles = "Admin")]
    public class RoleController( IRoleService roleService,RoleManager<AppRole> roleManager) : ControllerBase
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
    }
}
