using HR.Application.Dtos.RoleDtos;
using HR.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Application.Interfaces
{
    public interface IRoleService
    {
        public Task<RoleDto> GetRoleByNameAsync ( string roleName );
        public Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        public Task<RoleDto> CreateRoleAsync(CreateRoleDto roleDto);

        public Task<IEnumerable<RoleDto>> UpdateRoleAsync(UpdateRoleDto roleDto, string roleAction);
        public Task<IEnumerable<RoleDto>> DeleteRoleAsync(string roleName);

        Task<RoleUsersDto> GetRoleUsersAsync ( string RoleName );

    }
}
