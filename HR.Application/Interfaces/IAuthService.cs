using HR.Application.Dtos.AuthDtos;
using HR.Application.Dtos.RoleDtos;
using HR.Application.Response;
using HR.Domain.Models.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace HR.Application.Interfaces
{
 public interface IAuthService
 {

  Task<ApiResponse<UserProfile>> GetUserProfileAsync( string Username );
  Task<ApiResponse<List<string>>> GetAllUsersAsync( );
  Task<ApiResponse<AuthDto>> RegisterAsync( RegisterDto register );
  Task<ApiResponse<AuthDto>> LoginAsync( LoginDto login );
  Task<ApiResponse<(string message,bool isSuccess)>> AddRoleToUserAsync( AddRoleToUserDto addRoleToUser );
  Task<ApiResponse<(string message,bool isSuccess)>> RemoveUserRoleAsync( AddRoleToUserDto removeRoleFromUser );
  Task<ApiResponse<IList<string>>> GetUserRolesAsync( string Username );
  Task<ApiResponse<string>> LockUserAccountAsync( string Username );
  Task<ApiResponse<string>> UnlockUserAccountAsync( string Username );
  Task<ApiResponse<string>> ChangePasswordAsync( ChangePasswordDto changePasswordDto );
  Task<ApiResponse<string>> ActivateandDisActivateUserAccountAsync( string Username,bool isActivated );
  Task<ApiResponse<UserProfile>> UpdateUserProfileAsync( UserProfile userProfile );
  Task<ApiResponse<string>> DeleteUserAccountAsync( string Username );

 }
}
