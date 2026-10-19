using HR.Application.Dtos.AppDtos;
using HR.Application.Interfaces;
using HR.Application.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRBackEndApi.Controllers.AuthControllers
{
 [Route ("api/[controller]")]
 [ApiController]
 public class AuthController ( IAuthService authService,ILocalizationService localization ) : ControllerBase
 {
  /// <summary>
  /// Get the profile information of user
  /// </summary>
  /// <remarks>
  /// Returns basic profile information for the specified user. If the user does not exist a BadRequest with an error message is returned.
  /// </remarks>
  /// <example>
  /// GET /api/Auth/GetUserProfile/john.doe
  /// // Success response (200)
  /// { "success": true, "data": { "FirstName": "John", "LastName": "Doe", "Email": "john@example.com" }, "message": "" }
  /// // UnAuthorized response (401)
  /// { "success": false, "message": "User not found" }
  /// </example>
  /// <param name="username">The username to retrieve profile for.</param>
  /// <returns>ApiResponse containing the UserProfile when successful.</returns>
  [HttpGet ("GetUserProfile/{username}")]
  public async Task<ActionResult<ApiResponse<UserProfile>>> GetUserProfileAsync ( string username )
  {
   ApiResponse<UserProfile> result = await authService.GetUserProfileAsync (username);
   return (!result.Success) ? Unauthorized (result) : Ok (result);
  }
  /// <summary>
  /// Get all users in the system
  /// </summary>
  /// <remarks>
  /// Returns all usernames in the system. Use with caution in large deployments.
  /// </remarks>
  /// <example>
  /// GET /api/Auth/GetAllUsers
  /// // Success (200)
  /// { "success": true, "data": [ "jdoe", "asmith" ], "message": "" }
  /// </example>
  /// <returns>ApiResponse with list of usernames.</returns>
  [HttpGet ("GetAllUsers")]
  public async Task<ActionResult<ApiResponse<List<string>>>> GetAllUsers ( )
  {
   ApiResponse<List<string>> result = await authService.GetAllUsersAsync ();
   return (!result.Success) ? BadRequest (result) : Ok (result);
  }
  /// <summary>
  /// Registers a new user in the system
  /// </summary>
  /// <remarks>
  /// Creates a new user account. Returns Created (201) with AuthDto on success. If validation fails returns BadRequest with model errors.
  /// </remarks>
  /// <example>
  /// POST /api/Auth/RegisterUser
  /// { "Username": "jdoe", "FirstName": "John", "LastName": "Doe", "Email": "john@example.com", "Password": "P@ssw0rd" }
  /// // Success (201)
  /// { "success": true, "data": { "token": "...", "isAuthenticated": true }, "message": "" }
  /// // Validation error (400)
  /// { "success": false, "message": "validationerror", "data": { ... } }
  /// </example>
  /// <param name="register">Registration request body.</param>
  /// <returns>ApiResponse containing AuthDto when registration succeeds.</returns>
  [AllowAnonymous]
  [HttpPost ("RegisterUser")]
  public async Task<ActionResult<ApiResponse<AuthDto>>> RegisterAsync ( [FromBody] RegisterDto register )
  {
   if (!ModelState.IsValid)
   {
    return BadRequest (new ApiResponse<object>
    {
     Success=false,
     Message=localization.Get ("validationerror"),
     Data=ModelState,
     Language=localization.GetLanguage ()
    });
   }
   ApiResponse<AuthDto> result = await authService.RegisterAsync (register);
   return (!result.Success) ? BadRequest (result) : Created ("",result);
  }
  /// <summary>
  /// Logs in a user and returns an authentication token
  /// </summary>
  /// <remarks>
  /// Returns an authentication token and user info when credentials are valid. Returns Unauthorized (401) on invalid credentials.
  /// </remarks>
  /// <example>
  /// POST /api/Auth/Login
  /// { "Username": "jdoe", "Password": "P@ssw0rd" }
  /// // Success (200)
  /// { "success": true, "data": { "token": "...", "isAuthenticated": true }, "message": "" }
  /// // Unauthorized (401)
  /// { "success": false, "message": "invalid credentials" }
  /// </example>
  /// <param name="login">Login request payload.</param>
  /// <returns>ApiResponse with AuthDto when successful, 401 when credentials fail.</returns>
  [AllowAnonymous]
  [HttpPost ("Login")]
  public async Task<ActionResult<ApiResponse<AuthDto>>> Login ( [FromBody] LoginDto login )
  {
   if (!ModelState.IsValid)
   {
    return BadRequest (new ApiResponse<object>
    {
     Success=false,
     Message=localization.Get ("validationerror"),
     Data=ModelState,
     Language=localization.GetLanguage ()
    });
   }
   var result = await authService.LoginAsync (login);
   return (!result.Success) ? Unauthorized (result) : Ok (result);
  }
  /// <summary>
  /// Gets a list of roles assigned to a specific user
  /// </summary>
  /// <remarks>
  /// Returns the collection of role names for the given username. If the user is not found a BadRequest is returned.
  /// </remarks>
  /// <example>
  /// GET /api/Auth/GetUserRoles/jdoe
  /// // Success (200)
  /// { "success": true, "data": [ "Admin", "User" ], "message": "" }
  /// // Error (400)
  /// { "success": false, "message": "User not found" }
  /// </example>
  /// <param name="username">Username to query roles for.</param>
  /// <returns>ApiResponse with a list of role names.</returns>
  [HttpGet ("GetUserRoles/{username}")]
  public async Task<ActionResult<ApiResponse<IList<string>>>> GetUserRoles ( string username )
  {
   ApiResponse<IList<string>> result = await authService.GetUserRolesAsync (username);
   return (!result.Success) ? BadRequest (result) : Ok (result);
  }
  /// <summary>
  /// Adds a role to a user
  /// </summary>
  /// <remarks>
  /// Assigns the specified role to the named user. Returns BadRequest if validation fails or assignment is not possible.
  /// </remarks>
  /// <example>
  /// POST /api/Auth/AddRoleToUser
  /// { "UserName": "jdoe", "RoleName": "Admin" }
  /// // Success (200)
  /// { "success": true, "message": "Role assigned" }
  /// // BadRequest (400)
  /// { "success": false, "message": "validationerror" }
  /// </example>
  /// <param name="addRoleToUser">Role assignment request.</param>
  /// <returns>ApiResponse with operation message.</returns>
  [HttpPost ("AddRoleToUser")]
  public async Task<ActionResult<ApiResponse<string>>> AddRoleToUser ( [FromBody] AssignRoleDto addRoleToUser )
  {
   if (!ModelState.IsValid)
    return BadRequest (new ApiResponse<object>
    {
     Success=false,
     Message=localization.Get ("validationerror"),
     Data=ModelState,
     Language=localization.GetLanguage ()
    });
   var result = await authService.AddRoleToUserAsync (addRoleToUser);
   return (!result.Success) ? BadRequest (result) : Ok (result);
  }
  /// <summary>
  /// Removes a role from a user
  /// </summary>
  /// <param name="removeRoleFromUser"></param>
  /// <returns>
  /// A message indicating the result of the operation
  /// </returns>
  [HttpDelete ("RemoveRoleFromUser")]
  public async Task<ActionResult<ApiResponse<string>>> RemoveRoleFromUser ( [FromBody] AssignRoleDto removeRoleFromUser )
  {
   if (!ModelState.IsValid)
    return BadRequest (new ApiResponse<object>
    {
     Success=false,
     Message=localization.Get ("validationerror"),
     Data=ModelState,
     Language=localization.GetLanguage ()
    });
   var result = await authService.RemoveUserRoleAsync (removeRoleFromUser);
   return (!result.Success) ? BadRequest (result) : Ok (result);
  }
  /// <summary>
  /// Changes the password for a user
  /// </summary>
  /// <remarks>
  /// Validates current password and updates to the new password. Returns BadRequest on validation errors or failure.
  /// </remarks>
  /// <example>
  /// PUT /api/Auth/ChangePassword
  /// { "Username": "jdoe", "OldPassword": "old", "NewPassword": "new", "ConfirmPassword": "new" }
  /// // Success (200)
  /// { "success": true, "message": "Password changed" }
  /// // BadRequest (400)
  /// { "success": false, "message": "missingfields" }
  /// </example>
  /// <param name="changePasswordDto">Change password request.</param>
  /// <returns>ApiResponse with operation message.</returns>
  [HttpPut ("ChangePassword")]
  public async Task<ActionResult<ApiResponse<string>>> ChangePassword ( [FromBody] ChangePasswordDto changePasswordDto )
  {
   if (!ModelState.IsValid)
   {
    return BadRequest (new ApiResponse<object>
    {
     Success=false,
     Message=localization.Get ("missingfields"),
     Data=ModelState,
     Language=localization.GetLanguage ()
    });
   }
   ApiResponse<string> result = await authService.ChangePasswordAsync (changePasswordDto);
   return (!result.Success) ? BadRequest (result) : Ok (result);
  }
  /// <summary>
  /// Activates or deactivates a user account
  /// </summary>
  /// <param name="username"></param>
  /// <param name="isActivated"></param>
  /// <returns>
  /// A message indicating the result of the operation
  /// </returns>
  [HttpPut ("ActivateUserAccount/{username}")]
  public async Task<ActionResult<ApiResponse<string>>> ActivateUserAccount ( string username,bool isActivated )
  {
   ApiResponse<string> result = await authService.ActivateandDisActivateUserAccountAsync (username,isActivated);
   return (!result.Success) ? BadRequest (result) : Ok (result);
  }
  /// <summary>
  /// Locks a user account for a specified duration
  /// </summary>
  /// <param name="username"></param>
  /// <returns>
  /// A message indicating the result of the operation
  /// </returns>
  [HttpPut ("LockUserAccount/{username}")]
  public async Task<ActionResult<ApiResponse<string>>> LockUserAccount ( string username )
  {
   ApiResponse<string> result = await authService.LockUserAccountAsync (username);
   return (!result.Success) ? BadRequest (result) : Ok (result);
  }
  /// <summary>
  /// Unlocks a user account
  /// </summary>
  /// <param name="username"></param>
  /// <returns>
  /// A message indicating the result of the operation
  /// </returns>
  [HttpPut ("UnlockUserAccount/{username}")]
  public async Task<ActionResult<ApiResponse<string>>> UnlockUserAccount ( string username )
  {
   ApiResponse<string> result = await authService.UnlockUserAccountAsync (username);
   return (!result.Success) ? BadRequest (result) : Ok (result);
  }
  /// <summary>
  /// Updates the profile information of a user
  /// </summary>
  /// <param name="userProfile"></param>
  /// <returns>
  /// The updated user profile
  /// </returns>
  [HttpPut ("UpdateUserProfile")]
  public async Task<ActionResult<ApiResponse<UserProfile>>> UpdateUserProfile ( [FromBody] UserProfile userProfile )
  {
   if (!ModelState.IsValid)
   {
    return BadRequest (ModelState);
   }
   ApiResponse<UserProfile> result = await authService.UpdateUserProfileAsync (userProfile);
   return (!result.Success) ? BadRequest (result) : Ok (result);
  }
  /// <summary>
  /// Deletes a user account from the system
  /// </summary>
  /// <param name="username"></param>
  /// <returns>
  /// A message indicating the result of the operation
  /// </returns>
  [HttpDelete ("DeleteUserAccount/{username}")]
  public async Task<ActionResult<ApiResponse<string>>> DeleteUserAccount ( string username )
  {
   ApiResponse<string> result = await authService.DeleteUserAccountAsync (username);
   return (!result.Success) ? BadRequest (result) : Ok (result);
  }
  [HttpPost ("RevokeCurrentToken")]
  public async Task<ActionResult<ApiResponse<string>>> RevokeCurrentToken ( )
  {
   var response = new ApiResponse<string> { Success=false,Language=localization.GetLanguage () };
   var jti = User?.FindFirst (System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti)?.Value;
   if (string.IsNullOrEmpty (jti))
   {
    response.Message=localization.Get ("missingfields");
    return BadRequest (response);
   }

   var revocationService = HttpContext.RequestServices.GetService<HR.Application.Interfaces.ITokenRevocationService> ();
   if (revocationService==null)
   {
    response.Message="Revocation service unavailable";
    return StatusCode (500,response);
   }

   var expClaim = User?.FindFirst (System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Exp)?.Value;
   DateTime? expiresAtUtc = null;
   if (long.TryParse (expClaim,out var epoch))
    expiresAtUtc=DateTimeOffset.FromUnixTimeSeconds (epoch).UtcDateTime;

   await revocationService.RevokeAsync (jti,expiresAtUtc);
   response.Success=true;
   response.Message=localization.Get ("itemcreated");
   response.Data=jti;
   return Ok (response);
  }

 }
}
