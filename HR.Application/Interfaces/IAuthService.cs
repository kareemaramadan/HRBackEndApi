using HR.Application.Dtos.AuthDtos;
using HR.Domain.Models.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace HR.Application.Interfaces
{
    public interface IAuthService
    {
        //public Task<AuthDto> LoginAsync(LoginDto login);

        public Task<AuthDto> RegisterAsync(RegisterDto register);

       // public Task<JwtSecurityToken> CreateJwtToken(AppUser user);

        public Task<AuthDto> LoginAsync(LoginDto login);

    }
}
