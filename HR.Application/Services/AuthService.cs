using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Helpers;
using HR.Application.Interfaces;
using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace HR.Application.Services
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,IMapper mapper,IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
        }


        public async Task<AuthDto> RegisterAsync(RegisterDto register)
        {
            string message = string.Empty;
            if (await _userManager.FindByNameAsync(register.Username) is not null)
            {
                message = "Username already exists";
                return new AuthDto { Message = message };
            }
            else if (await _userManager.FindByEmailAsync(register.Email) is not null)
            {
                message = "Email already exists";
                return new AuthDto { Message = message };
            }

            AppUser user = _mapper.Map<AppUser>(register);

            IdentityResult result = await _userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}, ";
                }
                return new AuthDto { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, "User");

            JwtSecurityToken jwtSecurityToken = await _jwtTokenService.CreateNewToken(user);


            AuthDto AuthUser =  new AuthDto
            {
                UserId = (await _userManager.FindByNameAsync(user.UserName))?.Id,
                Email = user.Email,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Roles = (await _userManager.GetRolesAsync(user)).ToList(),
                CreatedOn = user.CreationTime,

            };

            return AuthUser;

        }
    }
}
