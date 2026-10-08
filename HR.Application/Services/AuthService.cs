using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Dtos.RoleDtos;
using HR.Application.Interfaces;
using HR.Application.Response;
using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace HR.Application.Services
{
    /// <summary>
    /// Service for handling user authentication, registration, role management, and account status operations.
    /// </summary>
    public class AuthService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager, IMapper mapper, IJwtTokenService jwtTokenService, ILocalizationService localization) : IAuthService
    {
        /// <summary>
        /// This function is used only in the AuthService for getting Appuser data
        /// </summary>
        /// <param name="Username"></param>
        /// <returns>
        /// The Appuser data
        /// </returns>
        private async Task<ApiResponse<AppUser>> GetUserDataAsync(string Username)
        {
            if (string.IsNullOrEmpty(Username))
                return new ApiResponse<AppUser>
                {
                    Success = false,
                    Message = localization.Get("usernamerequired"),
                    Data = null,
                    Language = localization.GetLanguage()
                };
            AppUser? user = await userManager.FindByNameAsync(Username) ?? null;
            if (user == null)
            {
                return new ApiResponse<AppUser>
                {
                    Success = false,
                    Message = localization.Get("usernameNotfound"),
                    Data = null,
                    Language = localization.GetLanguage()
                };
            }
            return new ApiResponse<AppUser>
            {
                Success = true,
                Message = localization.Get("itemRetrieved"),
                Data = user,
                Language = localization.GetLanguage()
            };
        }
        /// <summary>
        /// Retrieves user data based on the provided username. If the username is null or empty, an ArgumentException is thrown. 
        /// If the user is not found, null is returned.
        /// </summary>
        /// <param name="Username"></param>
        /// <returns>
        ///     The user data if found, otherwise null.
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<ApiResponse<UserProfile>> GetUserProfileAsync(string Username)
        {
            if (string.IsNullOrEmpty(Username))
                return new ApiResponse<UserProfile>
                {
                    Success = false,
                    Message = localization.Get("usernamerequired"),
                    Data = null,
                    Language = localization.GetLanguage()
                };
            AppUser? user = await userManager.FindByNameAsync(Username) ?? null;
            if (user == null)
            {
                return new ApiResponse<UserProfile>
                {
                    Success = false,
                    Message = localization.Get("usernameNotfound"),
                    Data = null,
                    Language = localization.GetLanguage()
                };
            }
            return new ApiResponse<UserProfile>
            {
                Success = true,
                Message = localization.Get("itemRetrieved"),
                Data = mapper.Map<UserProfile>(user),
                Language = localization.GetLanguage()
            };
        }
        /// <summary>
        /// Retrieves a list of all usernames in the system. If no users are found, an empty list is returned.
        /// </summary>
        /// <returns>
        /// A list of all usernames in the system.
        /// </returns>
        public async Task<ApiResponse<List<string>>> GetAllUsersAsync()
        {
            List<AppUser> users = await userManager.Users.ToListAsync();
            if (users.Count == 0)
            {
                return new ApiResponse<List<string>>
                {
                    Success = false,
                    Message = localization.Get("Nousersfound"),
                    Data = [],
                    Language = localization.GetLanguage()
                };
            }
            return new ApiResponse<List<string>>
            {
                Success = true,
                Message = localization.Get("itemsRetrieved"),
                Data = [.. users.Select(u => u.UserName!)],
                Language = localization.GetLanguage()
            };
        }
        /// <summary>
        /// Registers a new user with the provided registration details and then logs them in if registration is successful.
        /// </summary>
        /// <param name="register"></param>
        /// <returns>
        ///     An AuthDto containing the user's information, roles, token, and other authentication details.
        /// </returns>
        public async Task<ApiResponse<AuthDto>> RegisterAsync(RegisterDto register)
        {
            if (await userManager.FindByNameAsync(register.Username) is not null || await userManager.FindByEmailAsync(register.Email) is not null)
            {
                return new ApiResponse<AuthDto>
                {
                    Success = false,
                    Message = localization.Get("usernameoremailexists"),
                    Data = null,
                    Language = localization.GetLanguage()
                };
            }
            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new AppRole { Name = "User", Description = "Default role for all Users" });
            var user = mapper.Map<AppUser>(register);
            IdentityResult result = await userManager.CreateAsync(user, register.Password);
            await userManager.AddToRoleAsync(user, "User");
            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}, ";
                }
                return new ApiResponse<AuthDto>
                {
                    Success = false,
                    Message = errors,
                    Data = null,
                    Language = localization.GetLanguage()
                };
            }
            LoginDto login = new() { Username = register.Username, Password = register.Password };
            return await LoginAsync(login);
        }
        /// <summary>
        /// Authenticates a user based on the provided login credentials. If successful, it generates a JWT token for the user.
        /// </summary>
        /// <param name="login"></param>
        /// <returns>
        ///     An AuthDto containing the user's information, roles, token, and other authentication details.
        /// </returns>
        public async Task<ApiResponse<AuthDto>> LoginAsync(LoginDto login)
        {
            ApiResponse<AuthDto> errorsresponse = new()
            {
                Success = false,
                Data = null,
                Language = localization.GetLanguage()
            };

            if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                errorsresponse.Message = localization.Get("userInvalidCredentials");
                return errorsresponse;
            }
            ApiResponse<AppUser> appUser = await GetUserDataAsync(login.Username);
            switch (appUser.Data)
            {
                case null:
                    errorsresponse.Message = localization.Get("usernameNotfound");
                    return errorsresponse;
                case { IsActivatedAccount: false }:
                    errorsresponse.Message = localization.Get("useraccountdisabled");
                    return errorsresponse;
            }
            var UserResult = await signInManager.PasswordSignInAsync(appUser.Data, login.Password, true, lockoutOnFailure: true);
            if (UserResult.IsLockedOut)
            {
                errorsresponse.Message = localization.Get("userlocked");
                return errorsresponse;
            }
            if (!UserResult.Succeeded)
            {
                errorsresponse.Message = localization.Get("userInvalidCredentials");
                return errorsresponse;
            }
            return new ApiResponse<AuthDto>
            {
                Success = true,
                Message = string.Empty,

                Data = await jwtTokenService.CreateToken(appUser.Data),
                Language = localization.GetLanguage()
            };
        }
        /// <summary>
        /// Adds a role to a user.
        /// </summary>
        /// <param name="addRoleToUser"></param>
        /// <returns> 
        /// A message indicating the result of the operation, either success or an error message.
        /// </returns>
        public async Task<ApiResponse<IList<string>>> AddRoleToUserAsync(AddRoleToUserDto addRoleToUserDto)
        {
            ApiResponse<IList<string>> validationMessage = await ValidateUserAndRole(addRoleToUserDto, "add");
            return new ApiResponse<IList<string>>
            {
                Success = validationMessage.Success,
                Language = validationMessage.Language,
                Message = validationMessage.Message,
                Data = validationMessage.Data
            };
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
        public async Task<ApiResponse<IList<string>>> ValidateUserAndRole(AddRoleToUserDto addRoleToUserDto, string action)
        {
            ApiResponse<IList<string>> response = new()
            {
                Language = localization.GetLanguage()
            };
            ApiResponse<AppUser> user = await GetUserDataAsync(addRoleToUserDto.Username);
            AppRole? role = await roleManager.FindByNameAsync(addRoleToUserDto.RoleName);
            if (user.Data is null || role is null)
            {
                response.Success = false;
                response.Message = localization.Get("userroleNotfound");
                response.Data = null;
                return response;
            }
            if (user != null && role != null && action.Equals("add", StringComparison.CurrentCultureIgnoreCase))
            {
                if (!user.Data.IsActivatedAccount)
                {
                    response.Success = false;
                    response.Message = localization.Get("UserAccountNotActivated");
                    response.Data = null;
                    return response;
                }
                if (await userManager.IsInRoleAsync(user.Data, role.Name!))
                {
                    response.Success = false;
                    response.Message = localization.Get("UserAlreadyAssignedToRole");
                    response.Data = null;
                    return response;
                }
                IdentityResult addResult = await userManager.AddToRoleAsync(user.Data!, role: role.Name!);
                if (!addResult.Succeeded)
                {
                    response.Success = false;
                    response.Message = localization.Get("RoleAddFailed");
                    response.Data = null;
                    return response;
                }
                response.Success = true;
                response.Message = localization.Get("RoleAddedSuccessfully");
                response.Data =  [..userManager.GetRolesAsync(user.Data).Result];
                return response;
            }
            else
            {
                if (user!.Data.IsActivatedAccount)
                {
                    response.Success = false;
                    response.Message = localization.Get("UserAccountNotActivated");
                    response.Data = null;
                    return response;
                }
                IdentityResult removeResult = await userManager.RemoveFromRoleAsync(user.Data!, role: role!.Name!);
                if (!removeResult.Succeeded)
                {
                    response.Success = false;
                    response.Message = localization.Get("RoleRemoveFailed");
                    response.Data = null;
                    return response;
                }
                response.Success = true;
                response.Message = localization.Get("RoleRemovedSuccessfully");
                response.Data = [.. userManager.GetRolesAsync(user.Data).Result];
                return response;
            }
        }
        /// <summary>
        /// Removes a role from a user.
        /// </summary>
        /// <param name="removeRoleFromUser"></param>
        /// <returns>
        /// A message indicating the result of the operation, either success or an error message.
        /// </returns>
        public async Task<ApiResponse<IList<string>>> RemoveUserRoleAsync(AddRoleToUserDto removeRoleFromUser)
        {
            ApiResponse<IList<string>> validationMessage = await ValidateUserAndRole(removeRoleFromUser, "remove");
            return new ApiResponse<IList<string>>
            {
                Success = validationMessage.Success,
                Language = validationMessage.Language,
                Message = validationMessage.Message,
                Data = validationMessage.Data
            };
        }
        /// <summary>
        /// Retrieves a list of roles assigned to a specific user.
        /// </summary>
        /// <param name="Username"></param>
        /// <returns>
        /// A list of roles assigned to the specified user.
        /// </returns>
        public async Task<ApiResponse<IList<string>>> GetUserRolesAsync(string Username)
        {
            if (string.IsNullOrEmpty(Username))
            {
                return new ApiResponse<IList<string>>
                {
                    Success = false,
                    Message = localization.Get("usernamerequired"),
                    Data = null,
                    Language = localization.GetLanguage()
                };
            }
            ApiResponse<AppUser> userresponse = await GetUserDataAsync(Username);
            AppUser? user = userresponse.Data;
            if (user is null)
            {
                return new ApiResponse<IList<string>>
                {
                    Success = false,
                    Message = localization.Get("usernameNotfound"),
                    Data = null,
                    Language = localization.GetLanguage()
                };
            }
            IList<string> roles = await userManager.GetRolesAsync(user);
            if (roles.Count == 0)
            {

                return new ApiResponse<IList<string>>
                {
                    Success = false,
                    Message = localization.Get("NoUserAlreadyAssignedToRole"),
                    Data = null,
                    Language = localization.GetLanguage()
                };
            }
            return new ApiResponse<IList<string>>
            {
                Success = true,
                Message = localization.Get("itemsRetrieved"),
                Data = roles,
                Language = localization.GetLanguage()
            };
        }
        /// <summary>
        /// Locks a user's account for a specified duration (5 minutes in this case). This prevents the user from logging in during the lockout period.
        /// </summary>
        /// <param name="Username"></param>
        /// <returns>
        /// A boolean indicating whether the operation was successful.
        /// </returns>
        public async Task<ApiResponse<string>> LockUserAccountAsync(string Username)
        {
            if (string.IsNullOrEmpty(Username))
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = localization.Get("usernamerequired"),
                    Data = null,
                    Language = localization.GetLanguage()
                };
            }
            ApiResponse<AppUser> user = await GetUserDataAsync(Username);
            if (user.Data is null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = localization.Get("usernameNotfound"),
                    Data = null,
                    Language = localization.GetLanguage()
                };
            }
            user.Data.LockoutEnabled = true;
            user.Data.AccessFailedCount = 5;
            user.Data.IsActivatedAccount = false;
            // user.Data.LockoutEnd=DateTimeOffset.Now.Add(lockoutDuration);
            IdentityResult result = await userManager.UpdateAsync(user.Data);
            if (!result.Succeeded)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = localization.Get("userlockfailed"),
                    Data = null,
                    Language = localization.GetLanguage()
                };
            }
            return new ApiResponse<string>
            {
                Success = true,
                Message = localization.Get("userlocked"),
                Data = null,
                Language = localization.GetLanguage()
            };
        }
        /// <summary>
        /// Unlocks a user's account, allowing them to log in again. This method resets the lockout end time and access failed count for the user.
        /// </summary>
        /// <param name="Username"></param>
        /// <returns>
        /// A boolean indicating whether the operation was successful.
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<ApiResponse<string>> UnlockUserAccountAsync(string Username)
        {
            if (string.IsNullOrEmpty(Username))
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = localization.Get("usernamerequired"),
                    Data = null,
                    Language = localization.GetLanguage()
                };
            }
            ApiResponse<AppUser> user = await GetUserDataAsync(Username);
            if (user.Data is null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = localization.Get("usernameNotfound"),
                    Data = null,
                    Language = localization.GetLanguage()
                };
            }
            AppUser appUser = user.Data;
            appUser.LockoutEnabled = true;
            appUser.LockoutEnd = null;
            appUser.AccessFailedCount = 0;
            appUser.IsActivatedAccount = true;
            IdentityResult result = await userManager.UpdateAsync(appUser);
            if (!result.Succeeded)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = localization.Get("userunlockfailed"),
                    Data = null,
                    Language = localization.GetLanguage()
                };
            }
            return new ApiResponse<string>
            {
                Success = true,
                Message = localization.Get("userUnlocked"),
                Data = null,
                Language = localization.GetLanguage()
            };
        }
        /// <summary>
        /// Changes the password for a user. It first validates the input, checks if the new password and confirm password match, and then attempts to change the password using the UserManager. 
        /// If successful, it updates the security stamp of the user.
        /// </summary>
        /// <param name="changePasswordDto"></param>
        /// <returns>
        /// A string indicating the result of the operation.
        /// </returns>
        public async Task<ApiResponse<string>> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            ApiResponse<string> response = new()
            {
                Success = false,
                Data = null,
                Language = localization.GetLanguage()
            };
            if (string.IsNullOrEmpty(changePasswordDto.Username) || string.IsNullOrEmpty(changePasswordDto.OldPassword) || string.IsNullOrEmpty(changePasswordDto.NewPassword) || string.IsNullOrEmpty(changePasswordDto.ConfirmPassword))
            {
                response.Message = localization.Get("missingfields");
                return response;
            }
            if (changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
            {
                response.Message = localization.Get("passwordnotmatch");
                return response;
            }
         ;
            ApiResponse<AppUser> user = await GetUserDataAsync(changePasswordDto.Username);
            if (user.Data is null)
            {
                response.Message = localization.Get("usernameNotfound");
                return response;
            }
         ;
            var result = await userManager.ChangePasswordAsync(user.Data, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                response.Message = string.Join(", ", result.Errors.Select(e => e.Description));
                return response;
            }
            await userManager.UpdateSecurityStampAsync(user.Data);

            response.Message = (result.Succeeded) ? localization.Get("passwordchanged") : string.Join(", ", result.Errors.Select(e => e.Description));
            response.Success = true;
            return response;
        }
        /// <summary>
        /// Activates a user's account, allowing them to log in. 
        /// This method sets the IsActivatedAccount property of the user to true.
        /// </summary>
        /// <param name="Username"></param>
        /// <returns>
        /// A boolean indicating whether the operation was successful.
        /// </returns>
        public async Task<ApiResponse<string>> ActivateandDisActivateUserAccountAsync(string Username, bool isActivated)
        {
            ApiResponse<string> response = new()
            {
                Success = false,
                Data = null,
                Language = localization.GetLanguage()
            };
            if (string.IsNullOrEmpty(Username))
            {
                response.Message = localization.Get("usernamerequired");
                return response;
            }
         ;

            ApiResponse<AppUser> user = await GetUserDataAsync(Username);
            if (user.Data is null)
            {
                response.Message = localization.Get("usernameNotfound");
                return response;
            }
            user.Data.IsActivatedAccount = isActivated;
            IdentityResult result = await userManager.UpdateAsync(user.Data);
            response.Message = (result.Succeeded && isActivated) ? localization.Get("useraccountactivated") : localization.Get("useractivationfailed");
            response.Success = true;
            return response;
        }
        /// <summary>
        /// Updates the profile information of a user. It first validates the input, retrieves the existing user data,
        /// and then updates the user's profile with the provided information.
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns>
        /// A UserProfile object representing the updated user profile.
        /// </returns>
        public async Task<ApiResponse<UserProfile>> UpdateUserProfileAsync(UserProfile userProfile)
        {
            ApiResponse<UserProfile> response = new()
            {
                Success = false,
                Data = null,
                Language = localization.GetLanguage()
            };
            if (string.IsNullOrEmpty(userProfile.Username))
            {
                response.Message = localization.Get("usernamerequired");
                return response;
            }
            ApiResponse<AppUser> user = await GetUserDataAsync(userProfile.Username);
            if (user.Data is null)
            {
                response.Message = localization.Get("usernameNotfound");
                return response;
            }
            AppUser updatedUser = mapper.Map<AppUser>(userProfile);
            IdentityResult result = await userManager.UpdateAsync(updatedUser);
            if (!result.Succeeded)
            {
                response.Message = string.Join(", ", result.Errors.Select(e => e.Description));
                return response;
            }
            UserProfile profile = mapper.Map<UserProfile>(updatedUser);
            response.Data = profile;
            response.Success = true;
            response.Message = localization.Get("userprofileupdated");
            return response;
        }
        /// <summary>
        /// Deletes a user's account from the system. It first validates the input, retrieves the existing user data, and then attempts to delete the user using the UserManager.
        /// </summary>
        /// <param name="Username"></param>
        /// <returns>
        /// A string indicating the result of the operation.
        /// </returns>
        public async Task<ApiResponse<string>> DeleteUserAccountAsync(string Username)
        {
            ApiResponse<string> response = new()
            {
                Success = false,
                Data = null,
                Language = localization.GetLanguage()
            };
            if (string.IsNullOrEmpty(Username))
            {
                response.Message = localization.Get("usernamerequired");
                return response;
            }
         ;
            ApiResponse<AppUser> user = await GetUserDataAsync(Username);
            if (user.Data is null)
            {
                response.Message = localization.Get("usernameNotfound");
                return response;
            }
            IdentityResult result = await userManager.DeleteAsync(user.Data);
            response.Message = result.Succeeded ? localization.Get("userdeleted") : localization.Get("userdeletefailed");
            response.Success = true;
            return response;
        }

    }
}
