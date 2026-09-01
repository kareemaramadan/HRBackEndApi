using HR.Application.Dtos.AuthDtos;
using HR.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace HR.Application.Interfaces
{
    public interface IJwtTokenService
    {
        public Task<JwtSecurityToken> CreateNewJwtSecurityToken(AppUser user);
        public Task<AuthDto> CreateToken(AppUser user);
    }
}
