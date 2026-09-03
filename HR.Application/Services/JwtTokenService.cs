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
    /// <summary>
    /// Service for generating JWT tokens for authenticated users.
    /// </summary>
    public class JwtTokenService(UserManager<AppUser> userManager, JwtOptions jwtOptions, IMapper mapper) : IJwtTokenService
    {
        /// <summary>
        /// Creates a new JWT security token for the specified user, including their claims and roles.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        ///     A new JWT security token for the specified user.
        /// </returns>
        public async Task<JwtSecurityToken> CreateNewJwtSecurityToken(AppUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            if (userClaims is null)
            {
                var userClaim = new List<Claim>
                {
                    new ("UserName", user.UserName!),
                    new ("FullName", user.FullName),
                    new (ClaimTypes.NameIdentifier,user.Id),
                    new (ClaimTypes.Email,user.Email!)
                };
                userClaims = userClaim;
                await userManager.AddClaimsAsync(user, userClaim);
            }
            var userRoles = await userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in userRoles)
            {
                roleClaims.Add(new Claim("Roles", role));
            }
            var tokenClaims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            }
            .Union(userClaims)
            .Union(roleClaims);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
            var signCredintials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: tokenClaims,
                expires: DateTime.Today.AddMinutes(jwtOptions.ExpireAt),
                signingCredentials: signCredintials
                );
            return jwtSecurityToken;
        }
        /// <summary>
        /// Creates a token for the specified user after successful authentication, including their roles and other relevant information,
        /// and after generating the JWT security token using the CreateNewjwtSecurityToken method.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        ///     An AuthDto containing the user's information, roles, token, and other authentication details.
        /// </returns>
        public async Task<AuthDto> CreateToken(AppUser user)
        {
            JwtSecurityToken jwtSecurityToken = await CreateNewJwtSecurityToken(user);
            AuthDto AuthUser = new()
            {
                User = mapper.Map<UserRequest>(user),
                TokenExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Roles = [.. (await userManager.GetRolesAsync(user))],
                Message = user.UserName + " is Logged in Successfully"
            };
            return AuthUser;
        }
    }
}
