using AutoMapper;
using HR.Application.Dtos.AppDtos;
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
 /// <summary>
 /// Service for generating JWT tokens for authenticated users.
 /// </summary>
    public class JwtTokenService(UserManager<AppUser> userManager, JwtOptions jwtOptions, IMapper mapper) : IJwtTokenService
    {
        public async Task<JwtSecurityToken> CreateNewJwtSecurityToken(AppUser user)
        {
            var userClaims = (await userManager.GetClaimsAsync(user))?.ToList() ?? 
                new List<Claim>();
            if (!userClaims.Any())
            {
                // local default claims for the token only — do NOT persist to the user store here
                userClaims = new List<Claim>
        {
         new ("UserName", user.UserName!),
         new ("FullName", user.FullName ?? string.Empty),
         new (ClaimTypes.NameIdentifier, user.Id),
         new (ClaimTypes.Email, user.Email ?? string.Empty)
        };
            }

            var userRoles = await userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in userRoles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenClaims = new[] { new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) }
                             .Union(userClaims)
                             .Union(roleClaims);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key!));
            var signCredintials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: tokenClaims,
                // use UTC now for expiry
                expires: DateTime.UtcNow.AddMinutes((double)jwtOptions.ExpireAt!),
                signingCredentials: signCredintials
            );

            return jwtSecurityToken;
        }

        public async Task<AuthDto> CreateToken(AppUser user)
        {
            JwtSecurityToken jwtSecurityToken = await CreateNewJwtSecurityToken(user);
            AuthDto AuthUser = new()
            {
                User = mapper.Map<UserRequest>(user),
                TokenExpiresIn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Roles = [.. (await userManager.GetRolesAsync(user))],
                Message = user.UserName + " is Logged in Successfully"
            };
            return AuthUser;
        }
    }
}
