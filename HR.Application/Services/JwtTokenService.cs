using HR.Application.Helpers;
using HR.Application.Interfaces;
using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace HR.Application.Services
{
    public class JwtTokenService(UserManager<AppUser> userManager) : IJwtTokenService
    {

        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly JWTProp _jwt;

        public async Task<JwtSecurityToken> CreateNewToken(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in userRoles)
            {
                roleClaims.Add(new Claim("Roles", role));
            }

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            }
            .Union(userClaims)
            .Union(roleClaims);


            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signCredintials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.ExpireAt),
                signingCredentials: signCredintials
                );
            return jwtSecurityToken;

        }
    }
}
