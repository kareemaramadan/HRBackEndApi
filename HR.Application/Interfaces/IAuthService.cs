using HR.Application.Dtos.AuthDtos;
using HR.Application.Dtos.RoleDtos;
using HR.Domain.Models.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace HR.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthDto> RegisterAsync ( RegisterDto register );
        Task<AuthDto> LoginAsync ( LoginDto login );
        Task<string> AddRoleToUserAsync ( AddRoleToUserDto addRoleToUser );
        Task<string> RemoveUserRoleAsync ( AddRoleToUserDto removeRoleFromUser );
       
        Task<UserRolesDto> GetUserRolesAsync ( string Username );
        Task<string> ActivateUserAccountAsync ( string Username, bool isActivated );
        Task<string> LockUserAccountAsync ( string Username, TimeSpan lockoutDuration );
        Task<string> UnlockUserAccountAsync ( string Username );
        Task<string> ChangePasswordAsync ( ChangePasswordDto changePasswordDto );
        Task<string> DeleteUserAccountAsync ( string Username );
        Task<UserProfile> UpdateUserProfileAsync(UserProfile userProfile);
        Task<List<string?>> GetAllUsersAsync ();
    }
}
