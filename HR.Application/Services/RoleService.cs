using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Dtos.RoleDtos;
using HR.Application.Interfaces;
using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace HR.Application.Services
{
    public class RoleService ( RoleManager<AppRole> roleManager,UserManager<AppUser> userManager, IMapper mapper ) : IRoleService
    {

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync ( )
        {
            var roles = await roleManager.Roles.Where ( r => r.IsDeleted == false ).ToListAsync ( );
            return mapper.Map<IEnumerable<RoleDto>> ( roles );
        }

        public async Task<RoleDto> GetRoleByNameAsync ( string roleName )
        {
            string message = string.Empty;
            AppRole? role = await roleManager.FindByNameAsync ( roleName );
            if ( role == null )
            {
                message = "This role is not exists.";
                return new RoleDto { Message = message };
            }
            return mapper.Map<RoleDto> ( role );
        }

        public async Task<RoleDto> CreateRoleAsync ( CreateRoleDto Newrole )
        {
            string message = string.Empty;

            if ( await roleManager.RoleExistsAsync ( Newrole.Name ) )
            {
                message = "This Role already exists.";
                return new RoleDto { Message = message };
            }

            AppRole identityRole = mapper.Map<AppRole> ( Newrole );

            var result = await roleManager.CreateAsync ( identityRole );

            if ( !result.Succeeded )
            {
                string errors = string.Empty;
                foreach ( var error in result.Errors )
                {
                    errors += $"{error.Description}, ";
                }
                message = errors.TrimEnd ( ',', ' ' );
            }

            var createdRole = await roleManager.FindByNameAsync ( Newrole.Name );

            return result.Succeeded ? mapper.Map<RoleDto> ( createdRole ) : new RoleDto { Message = message };

        }

        public async Task<IEnumerable<RoleDto>> UpdateRoleAsync ( UpdateRoleDto updateRole, string roleAction )
        {
            string message = string.Empty;
            AppRole? existingRole = await roleManager.FindByNameAsync ( updateRole.CurrentRoleName );
            if ( existingRole != null )
            {
                UpdateRoleDto updateRoleDto = new UpdateRoleDto
                {
                    NewName = updateRole.NewName,
                    Description = existingRole.Description?? updateRole.Description,
                    IsActive = updateRole.IsActive,
                };

                if ( roleAction.ToLower ( ) == "update" )
                {
                    existingRole.Name = updateRoleDto.NewName;
                    existingRole.Description = updateRoleDto.Description;
                    existingRole.NormalizedName = updateRole.NewName.ToUpper ( );
                    existingRole.IsActive = updateRole.IsActive;

                }
                else
                {
                    existingRole.IsDeleted = true;
                    existingRole.DeletedAt = DateTime.UtcNow;
                }
                if ( existingRole.Name.ToUpper ( ) != updateRole.CurrentRoleName.ToUpper ( ) )
                {
                    if ( await roleManager.RoleExistsAsync ( updateRole.NewName ) )
                    {
                        message = "This newrole already exists.";
                        return new List<RoleDto> { new RoleDto { Message = message } };
                    }
                }

                var result = await roleManager.UpdateAsync ( existingRole );
                if ( result.Succeeded )
                {
                    return await GetAllRolesAsync ( );
                }
                else
                {
                    foreach ( var error in result.Errors )
                    {
                        message += $"{error.Description}, ";
                    }
                    message = message.TrimEnd ( ',', ' ' );
                }
            }
            return new List<RoleDto> { new RoleDto { Message = message } };

        }

        public async Task<IEnumerable<RoleDto>> DeleteRoleAsync ( string roleName )
        {
            string message = string.Empty;
            AppRole? role = await roleManager.FindByNameAsync ( roleName );
            if ( role != null )
            {
                UpdateRoleDto? updateRole = new UpdateRoleDto { CurrentRoleName = roleName };
                return await UpdateRoleAsync ( updateRole, "Delete" );
            }

            message = "Role not found.";
            return new List<RoleDto> { new RoleDto { Message = message } };
        }


        /// <summary>
        /// Retrieves a list of users assigned to a specific role. 
        /// It first checks if the role name is provided and then fetches the role and its associated users. 
        /// If the role or users are not found, appropriate error messages are returned.
        /// </summary>
        /// <param name="RoleName"></param>
        /// <returns>
        /// A list of usernames of users assigned to the specified role.
        /// </returns>
        public async Task<RoleUsersDto> GetRoleUsersAsync ( string RoleName )
        {
            if ( string.IsNullOrEmpty ( RoleName ) )
            {
                return new RoleUsersDto { IsSuccess = false, Users = [ ], ErrorMessage = "Role name is required." };
            }
            AppRole? role = await roleManager.FindByNameAsync ( RoleName );
            if ( role is null )
            {
                return new RoleUsersDto { IsSuccess = false, Users = [ ], ErrorMessage = "Role not found." };
            }
            IList<AppUser> users = await userManager.GetUsersInRoleAsync ( role.Name! );
            List<string> usernames = users.Select ( u => u.UserName! ).Where ( u => u != null ).ToList ( );

            return ( users is null ) ? new RoleUsersDto { IsSuccess = false, Users = [ ], ErrorMessage = "No users found in this Role" }
            : new RoleUsersDto { IsSuccess = true, Users = usernames, ErrorMessage = null };
        }


    }
}