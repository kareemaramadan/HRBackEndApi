using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Dtos.RoleDtos;
using HR.Application.Interfaces;
using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;


namespace HR.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;
        public RoleService(RoleManager<AppRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<string> CreateRoleAsync(CreateRoleDto Newrole)
        {
            //string message = string.Empty;

            //if (await _roleManager.RoleExistsAsync(Newrole.Name)) 
            //{
            //    message = "This Role already exists.";
            //    return message;
            //}
            
            //AppRole identityRole = _mapper.Map<AppRole>(Newrole);
            
            //IdentityResult result = await _roleManager.CreateAsync(identityRole);

            //if (!result.Succeeded)
            //{
            //    string errors = string.Empty;
            //    foreach (var error in result.Errors)
            //    {
            //        errors += $"{error.Description}, ";
            //    }
            //    return errors;
            //}

            return "This role is Created Successfully";

        }
    }
}
