using HR.Application.Dtos.AppDtos;
using HR.Domain.Models.Authorization;
using HR.Domain.Models.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace HR.Application.Interfaces
{
 public interface IJwtTokenService
 {
  Task<JwtSecurityToken> CreateNewJwtSecurityToken ( AppUser user );
  Task<AuthDto> CreateToken ( AppUser user );

  Task<RefreshToken> GenerateRefreshTokenAsync ( AppUser user );

  Task<AuthDto> RefreshTokenAsync ( string token );
  Task<bool> RevokeTokenAsync ( string token );

 }
}
