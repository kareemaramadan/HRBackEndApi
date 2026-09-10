using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Dtos.RoleDtos;
using HR.Application.Interfaces;
using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace HR.Application.Services
{
    public class RoleService ( RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, IMapper mapper ) : IRoleService
    {
        /// <summary>
        /// Retrieves all roles that are not marked as deleted from the database.
        /// </summary>
        /// <returns>A list of RoleDto objects representing the roles.</returns>
        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync ( )
        {
            var roles = await roleManager.Roles.ToListAsync ( );
            return mapper.Map<IEnumerable<RoleDto>> ( roles );
        }
        /// <summary>
        /// Retrieves a role by its name from the database. If the role does not exist, it returns a RoleDto with an appropriate message.
        /// </summary>
        /// <param name="roleName">The name of the role to retrieve.</param>
        /// <returns>A RoleDto object representing the role or an error message.</returns>
        public async Task<RoleDto> GetRoleByNameAsync ( string roleName )
        {
            AppRole? role = await roleManager.FindByNameAsync ( roleName );
            if ( role == null )
            {
                return new RoleDto { Message = "This role is not exists." };
            }
            return mapper.Map<RoleDto> ( role );
        }
        /// <summary>
        /// Creates a new role in the database. It first checks if a role with the same name already exists. If it does, it returns a RoleDto with an appropriate message. 
        /// If the creation is successful, it returns the created role as a RoleDto; 
        /// otherwise, it returns a RoleDto with error messages.
        /// </summary>
        /// <param name="Newrole"></param>
        /// <returns>
        /// A RoleDto object representing the created role or an error message.
        /// </returns>
        public async Task<(RoleDto newRole, bool isSuccess)> CreateRoleAsync ( CreateRoleDto Newrole )
        {
            string message = string.Empty;
            if ( await roleManager.RoleExistsAsync ( Newrole.Name ) )
            {
                return (new RoleDto { Message = "This Role already exists." }, false);
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

            return result.Succeeded ? (mapper.Map<RoleDto> ( createdRole ), true) : (new RoleDto { Message = message }, false);

        }
        /// <summary>
        /// Updates an existing role in the database. 
        /// It first checks if the role exists. 
        /// If it does, it updates the role's properties based on the provided UpdateRoleDto and the specified action (update,delete and recover).
        /// </summary>
        /// <param name="updateRole"></param>
        /// <param name="roleAction"></param>
        /// <returns>
        /// A list of RoleDto objects representing the updated roles or an error message.
        /// </returns>
        public async Task<IEnumerable<RoleDto>> UpdateRoleAsync ( UpdateRoleDto updateRole )
        {
            string message = string.Empty;

            AppRole? existingRole = await roleManager.FindByNameAsync ( updateRole.CurrentRoleName );

            if ( existingRole == null )
            {
                message = "This role is not found.";
                return new List<RoleDto> { new RoleDto { Message = message } };
            }
            UpdateRoleDto updateRoleDto = new UpdateRoleDto
            {
                NewName = ( updateRole.NewName == string.Empty ) ? existingRole.Name : updateRole.NewName,
                Description = ( updateRole.Description == string.Empty ) ? existingRole.Description : updateRole.Description
            };
            existingRole.Name = updateRoleDto.NewName;
            existingRole.Description = updateRoleDto.Description;
            existingRole.NormalizedName = updateRole.NewName.ToUpper ( );

            if ( existingRole.Name.ToUpper ( ) != updateRole.CurrentRoleName.ToUpper ( ) )
            {
                if ( await roleManager.RoleExistsAsync ( updateRole.NewName ) )
                {
                    return new List<RoleDto> { new RoleDto { Message = "This new role already exists." } };
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

            return new List<RoleDto> { new RoleDto { Message = message } };

        }
        /// <summary>
        /// Deletes a role from the database. 
        /// It first checks if the role name is provided and then fetches the role and its associated users.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>
        /// A tuple containing a message and a boolean indicating success.
        /// </returns>
        public async Task<(string message, bool isSuccess)> DeleteRoleAsync ( string roleName )
        {
            string message = string.Empty;

            if ( roleName == null )
            {
                return ("Role name is required.", false);
            }
            AppRole? role = await roleManager.FindByNameAsync ( roleName.ToUpper() );
            if ( role == null ) 
            { 
                return ("Role not found.", false); 
            }
            var usersInRole = await userManager.GetUsersInRoleAsync ( roleName.ToUpper ( ) );
            if (usersInRole.Count > 0)
            {
                return ("This role has users assigned to it.", false);
            }
            else 
            {        
                var result = await roleManager.DeleteAsync (role);

                if ( !result.Succeeded )
                {
                    return ("Error occurred while deleting the role.", false);
                }
                return ("The role is deleted successfully", true);
            }
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
        public async Task<IList<string>> GetRoleUsersAsync ( string RoleName )
        {
            if ( string.IsNullOrEmpty ( RoleName ) )
            {
                return [ "Role name is required." ];
            }
            AppRole? role = await roleManager.FindByNameAsync ( RoleName );
            if ( role is null )
            {
                return [ "Role not found." ];
            }
            IList<AppUser> users = await userManager.GetUsersInRoleAsync ( role.Name! );
            List<string> usernames = users.Select ( u => u.UserName! ).Where ( u => u != null ).ToList ( );

            return ( users is null ) ? [ "No users found in this Role" ] : usernames;
        }

        /// <summary>
        /// Removes all users from a specific role.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>
        /// A tuple containing a message and a boolean indicating success.
        /// </returns>
        public async Task<(string message, bool isSuccess)> RemoveUsersFromRoleAsync ( string roleName )        
        {
            if ( string.IsNullOrEmpty ( roleName ) )
            {
                return ("Role name is required.", false);
            }
            AppRole? role = await roleManager.FindByNameAsync ( roleName );
            if ( role is null )
            {
                return ("Role not found.", false);
            }
            IList<AppUser> usersInRole = await userManager.GetUsersInRoleAsync ( role.Name! );
            if ( usersInRole.Count == 0 )
            {
                return ("No users found in this Role.", false);
            }
            foreach ( var user in usersInRole )
            {
                var result = await userManager.RemoveFromRoleAsync ( user, role.Name! );
                if ( !result.Succeeded )
                {
                    return ($"Error occurred while removing user {user.UserName} from the role.", false);
                }
            }
            return ("All users have been removed from the role successfully.", true);

        }



    }
}