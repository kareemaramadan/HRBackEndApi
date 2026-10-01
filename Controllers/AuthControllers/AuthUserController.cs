using HR.Application.Dtos.AuthDtos;
using HR.Application.Dtos.RoleDtos;
using HR.Application.Interfaces;
using HR.Application.Response;
using Microsoft.AspNetCore.Mvc;

namespace HRBackEndApi.Controllers.AuthControllers
{
 [Route("api/[controller]")]
 [ApiController]
 public class AuthUserController( IAuthService authService,ILocalizationService localization ):ControllerBase
 {

  /// <summary>
  /// Get the profile information of user
  /// </summary>
  /// <param name="username"></param>
  /// <returns>
  /// get the user profile information
  /// </returns>
  [HttpGet("GetUserProfile/{username}")]
  public async Task<ActionResult<ApiResponse<UserProfile>>> GetUserProfileAsync( string username )
  {
   ApiResponse<UserProfile> result = await authService.GetUserProfileAsync(username);
   return (!result.Success) ? BadRequest(result) : Ok(result);
  }
  /// <summary>
  /// Get all users in the system
  /// </summary>
  /// <returns>
  /// A list of all users in the system
  /// </returns>
  [HttpGet("GetAllUsers")]
  public async Task<ActionResult<ApiResponse<List<string>>>> GetAllUsers( )
  {
   ApiResponse<List<string>> result = await authService.GetAllUsersAsync();
   return (!result.Success) ? BadRequest(result) : Ok(result);
  }
  /// <summary>
  /// Registers a new user in the system
  /// </summary>
  /// <param name="register"></param>
  /// <returns>
  /// The authentication result
  /// </returns>
  [HttpPost("RegisterUser")]
  public async Task<ActionResult<ApiResponse<AuthDto>>> RegisterAsync( [FromBody] RegisterDto register )
  {
   if(!ModelState.IsValid)
   {
    return BadRequest(new ApiResponse<object>
    {
     Success=false,
     Message=localization.Get("validationerror"),
     Data=ModelState,
     Language=localization.GetLanguage()
    });
   }
   ApiResponse<AuthDto> result = await authService.RegisterAsync(register);
   return (!result.Success) ? BadRequest(result) : Created("",result);
  }
  /// <summary>
  /// Logs in a user and returns an authentication token
  /// </summary>
  /// <param name="login"></param>
  /// <returns>
  /// The authentication token result
  /// </returns>
  [HttpPost("Login")]
  public async Task<ActionResult<ApiResponse<AuthDto>>> Login( [FromBody] LoginDto login )
  {
   if(!ModelState.IsValid)
   {
    return BadRequest(new ApiResponse<object>
    {
     Success=false,
     Message=localization.Get("validationerror"),
     Data=ModelState,
     Language=localization.GetLanguage()
    });
   }
   var result = await authService.LoginAsync(login);
   return (!result.Success) ? BadRequest(result) : Ok(result);
  }
  /// <summary>
  /// Gets a list of roles assigned to a specific user
  /// </summary>
  /// <param name="username"></param>
  /// <returns>
  /// A list of roles assigned to the specified user
  /// </returns>
  [HttpGet("GetUserRoles/{username}")]
  public async Task<ActionResult<ApiResponse<IList<string>>>> GetUserRoles( string username )
  {
   ApiResponse<IList<string>> result = await authService.GetUserRolesAsync(username);
   return (!result.Success) ? BadRequest(result) : Ok(result);
  }
  /// <summary>
  /// Adds a role to a user
  /// </summary>
  /// <param name="addRoleToUser"></param>
  /// <returns>
  /// A message indicating the result of the operation
  /// </returns>
  [HttpPost("AddRoleToUser")]
  public async Task<ActionResult<ApiResponse<string>>> AddRoleToUser( [FromBody] AddRoleToUserDto addRoleToUser )
  {
   if(!ModelState.IsValid)
    return BadRequest(new ApiResponse<object>
    {
     Success=false,
     Message=localization.Get("validationerror"),
     Data=ModelState,
     Language=localization.GetLanguage()
    });
   var result = await authService.AddRoleToUserAsync(addRoleToUser);
   return (!result.Success) ? BadRequest(result) : Ok(result);
  }
  /// <summary>
  /// Removes a role from a user
  /// </summary>
  /// <param name="removeRoleFromUser"></param>
  /// <returns>
  /// A message indicating the result of the operation
  /// </returns>
  [HttpDelete("RemoveRoleFromUser")]
  public async Task<ActionResult<ApiResponse<string>>> RemoveRoleFromUser( [FromBody] AddRoleToUserDto removeRoleFromUser )
  {
   if(!ModelState.IsValid)
    return BadRequest(new ApiResponse<object>
    {
     Success=false,
     Message=localization.Get("validationerror"),
     Data=ModelState,
     Language=localization.GetLanguage()
    });
   var result = await authService.RemoveUserRoleAsync(removeRoleFromUser);
   return (!result.Success) ? BadRequest(result) : Ok(result);
  }
  /// <summary>
  /// Changes the password for a user
  /// </summary>
  /// <param name="changePasswordDto"></param>
  /// <returns>
  /// A message indicating the result of the operation
  /// </returns>
  [HttpPut("ChangePassword")]
  public async Task<ActionResult<ApiResponse<string>>> ChangePassword( [FromBody] ChangePasswordDto changePasswordDto )
  {
   if(!ModelState.IsValid)
   {
    return BadRequest(new ApiResponse<object>
    {
     Success=false,
     Message=localization.Get("missingfields"),
     Data=ModelState,
     Language=localization.GetLanguage()
    });
   }
   ApiResponse<string> result = await authService.ChangePasswordAsync(changePasswordDto);
   return (!result.Success) ? BadRequest(result) : Ok(result);
  }
  /// <summary>
  /// Activates or deactivates a user account
  /// </summary>
  /// <param name="username"></param>
  /// <param name="isActivated"></param>
  /// <returns>
  /// A message indicating the result of the operation
  /// </returns>
  [HttpPut("ActivateUserAccount/{username}")]
  public async Task<ActionResult<ApiResponse<string>>> ActivateUserAccount( string username,bool isActivated )
  {
   ApiResponse<string> result = await authService.ActivateandDisActivateUserAccountAsync(username,isActivated);
   return (!result.Success) ? BadRequest(result) : Ok(result);
  }
  /// <summary>
  /// Locks a user account for a specified duration
  /// </summary>
  /// <param name="username"></param>
  /// <returns>
  /// A message indicating the result of the operation
  /// </returns>
  [HttpPut("LockUserAccount/{username}")]
  public async Task<ActionResult<ApiResponse<string>>> LockUserAccount( string username )
  {
   ApiResponse<string> result = await authService.LockUserAccountAsync(username);
   return (!result.Success) ? BadRequest(result) : Ok(result);
  }
  /// <summary>
  /// Unlocks a user account
  /// </summary>
  /// <param name="username"></param>
  /// <returns>
  /// A message indicating the result of the operation
  /// </returns>
  [HttpPut("UnlockUserAccount/{username}")]
  public async Task<ActionResult<ApiResponse<string>>> UnlockUserAccount( string username )
  {
   ApiResponse<string> result = await authService.UnlockUserAccountAsync(username);
   return (!result.Success) ? BadRequest(result) : Ok(result);
  }
  /// <summary>
  /// Updates the profile information of a user
  /// </summary>
  /// <param name="userProfile"></param>
  /// <returns>
  /// The updated user profile
  /// </returns>
  [HttpPut("UpdateUserProfile")]
  public async Task<ActionResult<ApiResponse<UserProfile>>> UpdateUserProfile( [FromBody] UserProfile userProfile )
  {
   if(!ModelState.IsValid)
   {
    return BadRequest(ModelState);
   }
   ApiResponse<UserProfile> result = await authService.UpdateUserProfileAsync(userProfile);
   return (!result.Success) ? BadRequest(result) : Ok(result);
  }
  /// <summary>
  /// Deletes a user account from the system
  /// </summary>
  /// <param name="username"></param>
  /// <returns>
  /// A message indicating the result of the operation
  /// </returns>
  [HttpDelete("DeleteUserAccount/{username}")]
  public async Task<ActionResult<ApiResponse<string>>> DeleteUserAccount( string username )
  {
   ApiResponse<string> result = await authService.DeleteUserAccountAsync(username);
   return (!result.Success) ? BadRequest(result) : Ok(result);
  }

 }
}