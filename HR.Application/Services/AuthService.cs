using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Interfaces;
using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;


namespace HR.Application.Services
{
    /// <summary>
    /// Service for handling user authentication, including registration and login.
    /// </summary>
    public class AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper, IJwtTokenService jwtTokenService) : IAuthService
    {

        /// <summary>
        /// Registers a new user with the provided registration details and then logs them in if registration is successful.
        /// </summary>
        /// <param name="register"></param>
        /// <returns>
        ///     An AuthDto containing the user's information, roles, token, and other authentication details.
        /// </returns>
        public async Task<AuthDto> RegisterAsync(RegisterDto register)
        {
            string message = string.Empty;
            if (await userManager.FindByNameAsync(register.Username) is not null || await userManager.FindByEmailAsync(register.Email) is not null)
            {
                message = "Username or Email already exists";
                return new AuthDto { Message = message };
            }

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
                return new AuthDto { Message = errors };
            }

            LoginDto login = new LoginDto { Username = register.Username, Password = register.Password };

            return await LoginAsync(login);

        }

        /// <summary>
        /// Authenticates a user based on the provided login credentials. If successful, it generates a JWT token for the user.
        /// </summary>
        /// <param name="login"></param>
        /// <returns>
        ///     An AuthDto containing the user's information, roles, token, and other authentication details.
        /// </returns>

        public async Task<AuthDto> LoginAsync(LoginDto login)
        {
            string message = string.Empty;
            if(string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                message = "Username and Password are required";
                return new AuthDto { Message = message };
            }
            AppUser? appUser = await userManager.FindByNameAsync(login.Username);
            if (appUser is null || !await userManager.CheckPasswordAsync(appUser, login.Password))
            {
                message = "Username or Password is not Correct";
                return new AuthDto { Message = message };
            }
            else if (!appUser.IsActivatedAccount)
            {
                message = "Sorry,This account is disabled";
                return new AuthDto { Message = message };
            }
            else 
            {
                var UserResult = await signInManager.PasswordSignInAsync(appUser, login.Password, false, lockoutOnFailure: false);       
                if (UserResult.IsLockedOut)
                {
                    message = "User account is locked out.";           
                    return new AuthDto { Message = message };
                }
                return await jwtTokenService.CreateToken(appUser);
            }
        }

    }
}
