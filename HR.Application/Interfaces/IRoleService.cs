using HR.Application.Dtos.RoleDtos;
using HR.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Application.Interfaces
{
    public interface IRoleService
    {
        Task<RoleDto> GetRoleByNameAsync ( string roleName );
        Task<IEnumerable<RoleDto>> GetAllRolesAsync ( );
        Task<(RoleDto newRole, bool isSuccess)> CreateRoleAsync ( CreateRoleDto roleDto );
        Task<IEnumerable<RoleDto>> UpdateRoleAsync ( UpdateRoleDto roleDto);
        Task<(string message, bool isSuccess)> DeleteRoleAsync ( string roleName );
        Task<IList<string>> GetRoleUsersAsync ( string RoleName );
        Task<(string message, bool isSuccess)> RemoveUsersFromRoleAsync (string roleName);

    }
}
