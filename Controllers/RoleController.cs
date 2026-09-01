using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Dtos.RoleDtos;
using HR.Application.Interfaces;
using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRBackEndApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;
        public RoleController(IRoleService roleService,RoleManager<AppRole> roleManager,IMapper mapper)
        {
            _roleService = roleService;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> createRoleAsync([FromBody] CreateRoleDto role)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var roleDto = await _roleService.CreateRoleAsync(role);

            return Ok(roleDto);
            

        }
    }
}
