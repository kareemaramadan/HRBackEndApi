using HR.Application.Dtos.RoleDtos;
using HR.Application.Response;
using HR.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Application.Interfaces
{
    public interface IRoleService
    {
        Task<ApiResponse<RoleDto>> GetRoleByNameAsync ( string roleName );
        Task<ApiResponse<IEnumerable<RoleDto>>> GetAllRolesAsync ( );
        Task<ApiResponse<IEnumerable<RoleDto>>> CreateRoleAsync ( CreateRoleDto roleDto );
        Task<ApiResponse<IEnumerable<RoleDto>>> UpdateRoleAsync ( UpdateRoleDto roleDto);
        Task<ApiResponse<IEnumerable<RoleDto>>> DeleteRoleAsync ( string roleName );
        Task<ApiResponse<IList<string>>> GetRoleUsersAsync ( string RoleName );
        Task<ApiResponse<IList<AppUser>>> RemoveUsersFromRoleAsync (string roleName);

    }
}
