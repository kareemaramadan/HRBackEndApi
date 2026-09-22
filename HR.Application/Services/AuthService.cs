using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Dtos.RoleDtos;
using HR.Application.Interfaces;
using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;


namespace HR.Application.Services
{
    /// <summary>
    /// Service for handling user authentication, registration, role management, and account status operations.
    /// </summary>
    public class AuthService ( UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager, IMapper mapper, IJwtTokenService jwtTokenService ) : IAuthService
    {
        /// <summary>
        /// Retrieves user data based on the provided username. If the username is null or empty, an ArgumentException is thrown. 
        /// If the user is not found, null is returned.
        /// </summary>
        /// <param name="Username"></param>
        /// <returns>
        ///     The user data if found, otherwise null.
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<AppUser?> GetUserDataAsync ( string Username )
        {
            if ( string.IsNullOrEmpty ( Username ) )
            {
                throw new ArgumentException ( "Username is required", nameof ( Username ) );
            }
            AppUser? user = await userManager.FindByNameAsync ( Username ) ?? null;
            return user;
        }

        /// <summary>
        /// Registers a new user with the provided registration details and then logs them in if registration is successful.
        /// </summary>
        /// <param name="register"></param>
        /// <returns>
        ///     An AuthDto containing the user's information, roles, token, and other authentication details.
        /// </returns>
        public async Task<AuthDto> RegisterAsync ( RegisterDto register )
        {

            if ( await userManager.FindByNameAsync ( register.Username ) is not null || await userManager.FindByEmailAsync ( register.Email ) is not null )
            {
                return new AuthDto { Message = "Username or Email already exists" };
            }

            if ( !await roleManager.RoleExistsAsync ( "User" ) )
            {
                await roleManager.CreateAsync ( new AppRole { Name = "User", Description = "Default role for all Users" } );
            }

            var user = mapper.Map<AppUser> ( register );

            IdentityResult result = await userManager.CreateAsync ( user, register.Password );

            await userManager.AddToRoleAsync ( user, "User" );

            if ( !result.Succeeded )
            {
                string errors = string.Empty;
                foreach ( var error in result.Errors )
                {
                    errors += $"{error.Description}, ";
                }
                return new AuthDto { Message = errors };
            }

            LoginDto login = new ( ) { Username = register.Username, Password = register.Password };

            return await LoginAsync ( login );

        }

        /// <summary>
        /// Authenticates a user based on the provided login credentials. If successful, it generates a JWT token for the user.
        /// </summary>
        /// <param name="login"></param>
        /// <returns>
        ///     An AuthDto containing the user's information, roles, token, and other authentication details.
        /// </returns>
        public async Task<AuthDto> LoginAsync ( LoginDto login )
        {

            if ( string.IsNullOrEmpty ( login.Username ) || string.IsNullOrEmpty ( login.Password ) )
            {
                return new AuthDto { Message = "Username and Password are required" };
            }
            AppUser? appUser = await GetUserDataAsync ( login.Username );
            if ( appUser == null )
            {
                return new AuthDto { Message = "User not found" };
            }
            if ( !appUser.IsActivatedAccount )
            {
                return new AuthDto { Message = "Sorry,This account is disabled" };
            }
            var UserResult = await signInManager.PasswordSignInAsync ( appUser, login.Password, true, lockoutOnFailure: true );
            if ( UserResult.IsLockedOut )
            {
                return new AuthDto { Message = "User account is locked out." };
            }
            if ( !UserResult.Succeeded )
            {
                return new AuthDto { Message = "Username or Password is not Correct" };
            }

            return await jwtTokenService.CreateToken ( appUser );

        }

        /// <summary>
        /// Adds a role to a user.
        /// </summary>
        /// <param name="addRoleToUser"></param>
        /// <returns> 
        /// A message indicating the result of the operation, either success or an error message.
        /// </returns>
        public async Task<(string message, bool isSuccess)> AddRoleToUserAsync ( AddRoleToUserDto addRoleToUser )
        {

            var validationMessage = await ValidateUserAndRole ( addRoleToUser, "add" );

            // return ( validationMessage is null ) ?  "Role added to user successfully." : validationMessage;

            return validationMessage;
        }

        /// <summary>
        /// Validates the existence of a user and role, checks if the user is activated, and determines if the user is already assigned to the role. 
        /// Depending on the action ("add" or "remove"), it either adds or removes the role from the user.
        /// </summary>
        /// <param name="addRoleToUser"></param>
        /// <param name="action"></param>
        /// <returns>
        /// A message indicating the result of the validation, either null (if valid) or an error message.
        /// </returns>

        public async Task<(string message, bool isSuccess)> ValidateUserAndRole ( AddRoleToUserDto addRoleToUser, string action )
        {
            AppUser? user = await GetUserDataAsync ( addRoleToUser.Username );
            AppRole? role = await roleManager.FindByNameAsync ( addRoleToUser.RoleName );

            if ( user is null || role is null )
            {
                return ("User or Role is not found", false);
            }

            if ( user != null && role != null && action.Equals ( "add", StringComparison.CurrentCultureIgnoreCase ))
            {
                string? validationMessage = ( !user.IsActivatedAccount ) ? "User account is not activated" :
                await userManager.IsInRoleAsync ( user, role.Name! ) ? "User already assigned to this role" : null;

                if ( validationMessage != null )
                {
                    return (validationMessage, false);
                }
                IdentityResult addResult = await userManager.AddToRoleAsync ( user!, role: role.Name! );
                return addResult.Succeeded ? ("Role added to user successfully.", true) : ("Failed to add role to user.", false);
            }
            else
            {
                IdentityResult removeResult = await userManager.RemoveFromRoleAsync ( user!, role: role.Name! );
                return removeResult.Succeeded ? ("Role removed from user successfully.", true) : ("Failed to remove role from user.", false);
            }
        }

        /// <summary>
        /// Removes a role from a user.
        /// </summary>
        /// <param name="removeRoleFromUser"></param>
        /// <returns>
        /// A message indicating the result of the operation, either success or an error message.
        /// </returns>
        public async Task<(string message, bool isSuccess)> RemoveUserRoleAsync ( AddRoleToUserDto removeRoleFromUser )
        {
            var validationMessage = await ValidateUserAndRole ( removeRoleFromUser, "remove" );
            return validationMessage;
        }

        /// <summary>
        /// Retrieves a list of roles assigned to a specific user.
        /// </summary>
        /// <param name="Username"></param>
        /// <returns>
        /// A list of roles assigned to the specified user.
        /// </returns>
        public async Task<IList<string>> GetUserRolesAsync ( string Username )
        {
            if ( string.IsNullOrEmpty ( Username ) )
            {
                return [ "Username is required" ];
            }
            AppUser? user = await GetUserDataAsync ( Username );
            if ( user == null )
            {
                return [ "User not found" ];
            }
            IList<string> roles = await userManager.GetRolesAsync ( user );
            if ( roles.Count == 0 )
            {
                return [ "User has no roles assigned" ];
            }
            return roles;
        }

        /// <summary>
        /// Locks a user's account for a specified duration (5 minutes in this case). This prevents the user from logging in during the lockout period.
        /// </summary>
        /// <param name="Username"></param>
        /// <returns>
        /// A boolean indicating whether the operation was successful.
        /// </returns>
        public async Task<string> LockUserAccountAsync ( string Username, TimeSpan lockoutDuration )
        {
            if ( string.IsNullOrEmpty ( Username ) )
            {
                return "Username is required";
            }
            AppUser? user = await GetUserDataAsync ( Username );
            if ( user == null )
            {
                return "User not found";
            }
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.Now.Add ( lockoutDuration );
            IdentityResult result = await userManager.UpdateAsync ( user );
            return result.Succeeded ? "User account locked successfully" : "Failed to lock user account";
        }

        /// <summary>
        /// Unlocks a user's account, allowing them to log in again. This method resets the lockout end time and access failed count for the user.
        /// </summary>
        /// <param name="Username"></param>
        /// <returns>
        /// A boolean indicating whether the operation was successful.
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<string> UnlockUserAccountAsync ( string Username )
        {
            if ( string.IsNullOrEmpty ( Username ) )
            {
                return "Username is required";
            }
            AppUser? user = await GetUserDataAsync ( Username );
            user.LockoutEnabled = true;
            user.LockoutEnd = null;
            user.AccessFailedCount = 0;
            IdentityResult result = await userManager.UpdateAsync ( user );
            return result.Succeeded ? "User account unlocked successfully" : "Failed to unlock user account";
        }

        /// <summary>
        /// Changes the password for a user. It first validates the input, checks if the new password and confirm password match, and then attempts to change the password using the UserManager. 
        /// If successful, it updates the security stamp of the user.
        /// </summary>
        /// <param name="changePasswordDto"></param>
        /// <returns>
        /// A string indicating the result of the operation.
        /// </returns>

        public async Task<string> ChangePasswordAsync ( ChangePasswordDto changePasswordDto )
        {
            if ( string.IsNullOrEmpty ( changePasswordDto.Username ) || string.IsNullOrEmpty ( changePasswordDto.OldPassword ) || string.IsNullOrEmpty ( changePasswordDto.NewPassword ) || string.IsNullOrEmpty ( changePasswordDto.ConfirmPassword ) )
            {
                return "All fields are required";
            }
            if ( changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword )
            {
                return "New password and confirm password do not match";
            }
            AppUser? user = await GetUserDataAsync ( changePasswordDto.Username );

            if ( user == null )
            {
                return "User not found";
            }

            var result = await userManager.ChangePasswordAsync ( user, changePasswordDto.OldPassword, changePasswordDto.NewPassword );
            if ( !result.Succeeded )
            {
                return string.Join ( ", ", result.Errors.Select ( e => e.Description ) );
            }
            await userManager.UpdateSecurityStampAsync ( user );

            return result.Succeeded ? "Password changed successfully" : string.Join ( ", ", result.Errors.Select ( e => e.Description ) );
        }

        /// <summary>
        /// Activates a user's account, allowing them to log in. 
        /// This method sets the IsActivatedAccount property of the user to true.
        /// </summary>
        /// <param name="Username"></param>
        /// <returns>
        /// A boolean indicating whether the operation was successful.
        /// </returns>
        /// 
        public async Task<string> ActivateUserAccountAsync ( string Username, bool isActivated )
        {
            if ( string.IsNullOrEmpty ( Username ) )
            {
                return "Username is required";
            }
            AppUser? user = await GetUserDataAsync ( Username );
            user.IsActivatedAccount = isActivated;
            IdentityResult result = await userManager.UpdateAsync ( user );
            return ( result.Succeeded && isActivated ) ? "User account activated successfully" : "Failed to activate user account";
        }

        /// <summary>
        /// Deletes a user's account from the system. It first validates the input, retrieves the existing user data, and then attempts to delete the user using the UserManager.
        /// </summary>
        /// <param name="Username"></param>
        /// <returns>
        /// A string indicating the result of the operation.
        /// </returns>
        public async Task<string> DeleteUserAccountAsync ( string Username )
        {
            if ( string.IsNullOrEmpty ( Username ) )
            {
                return "Username is required";
            }
            AppUser? user = await GetUserDataAsync ( Username );
            if ( user == null )
            {
                return "User not found";
            }
            IdentityResult result = await userManager.DeleteAsync ( user );
            return result.Succeeded ? "User account deleted successfully" : "Failed to delete user account";
        }

        /// <summary>
        /// Updates the profile information of a user. It first validates the input, retrieves the existing user data,
        /// and then updates the user's profile with the provided information.
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns>
        /// A UserProfile object representing the updated user profile.
        /// </returns>
        public async Task<UserProfile> UpdateUserProfileAsync ( UserProfile userProfile )
        {
            if ( string.IsNullOrEmpty ( userProfile.Username ) )
            {
                return new UserProfile { Username = "Username is required" };
            }
            AppUser? user = await GetUserDataAsync ( userProfile.Username );
            if ( user == null )
            {
                return new UserProfile { Username = "User not found" };
            }
            new UserProfile
            {
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Email = userProfile.Email,
                NormalizedEmail = userProfile.Email.ToUpperInvariant ( ),
                Username = userProfile.Username,
                NormalizedUsername = userProfile.Username.ToUpperInvariant ( ),
                PhoneNumber = userProfile.PhoneNumber,
                ProfilePicture = userProfile.ProfilePicture
            };

            AppUser updatedUser = mapper.Map<AppUser> ( userProfile );

            await userManager.UpdateAsync ( updatedUser );

            UserProfile profile = mapper.Map<UserProfile> ( updatedUser );

            return profile;

        }

        /// <summary>
        /// Retrieves a list of all usernames in the system. If no users are found, an empty list is returned.
        /// </summary>
        /// <returns>
        /// A list of all usernames in the system.
        /// </returns>
        public async Task<List<string?>> GetAllUsersAsync ( )
        {
            List<AppUser> users = await userManager.Users.ToListAsync ( );
            return ( users is null ) ? [ ] : users.Select ( u => u.UserName ).ToList ( );
        }
    }
}
