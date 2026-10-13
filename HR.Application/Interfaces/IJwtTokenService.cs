using HR.Application.Dtos.AppDtos;
using HR.Domain.Models.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace HR.Application.Interfaces
{
 public interface IJwtTokenService
 {
  public Task<JwtSecurityToken> CreateNewJwtSecurityToken ( AppUser user );
  public Task<AuthDto> CreateToken ( AppUser user );
 }
}
