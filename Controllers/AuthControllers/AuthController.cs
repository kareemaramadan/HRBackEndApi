using HR.Application.Dtos.AppDtos;
using HR.Application.Interfaces;
using HR.Application.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRBackEndApi.Controllers.AuthControllers
{
 [Authorize]
 [Route ("api/[controller]")]
 [ApiController]
 public class AuthController ( IAuthService authService,ILocalizationService localization,IJwtTokenService jwtTokenService ) : ControllerBase
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
  /// <response code="200">User profile retrieved successfully</response>
  /// <response code="401">Unauthorized - missing or invalid authentication token</response>
  /// <response code="404">User not found - username does not exist</response>

  [HttpGet ("UserProfile/{username}")]
  [ProducesResponseType (typeof (ApiResponse<UserProfile>),StatusCodes.Status200OK)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status404NotFound)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status401Unauthorized)]
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
  /// <returns>ApiResponse containing list of usernames as strings</returns>
  /// <response code="200">Users list retrieved successfully</response>
  /// <response code="400">Unable to retrieve users - database error</response>
  /// <response code="401">Unauthorized - authentication required</response>

  [HttpGet ("AllUsers")]
  [ProducesResponseType (typeof (ApiResponse<List<string>>),StatusCodes.Status200OK)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status400BadRequest)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status401Unauthorized)]
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
  /// <response code="201">User registered successfully, account created</response>
  /// <response code="400">Validation failed - see data field for specific errors</response>

  [AllowAnonymous]
  [HttpPost ("Register")]
  [Consumes ("application/json")]
  [ProducesResponseType (typeof (ApiResponse<AuthDto>),StatusCodes.Status201Created)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status400BadRequest)]
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
   if (!result.Data!.IsAuthenticated)
    return BadRequest (result);
   else
   {
    SetRefreshTokenInCookies ("accessToken",result.Data!.Token!,result.Data.TokenExpiresIn);
    SetRefreshTokenInCookies ("refreshToken",result.Data!.RefreshToken!,result.Data.RefreshTokenExpiration);
    return Created ("",result);
   }
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
  /// <response code="200">Login successful, tokens provided</response>
  /// <response code="400">Validation failed - check required fields</response>
  /// <response code="401">Invalid credentials - username or password incorrect</response>

  [AllowAnonymous]
  [HttpPost ("Login")]
  [Consumes ("application/json")]
  [ProducesResponseType (typeof (ApiResponse<AuthDto>),StatusCodes.Status200OK)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status400BadRequest)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status401Unauthorized)]
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
   if (!result.Success)
   {
    return Unauthorized (result);
   }
   else
   {
    SetRefreshTokenInCookies ("accessToken",result.Data!.Token!,result.Data.TokenExpiresIn);
    SetRefreshTokenInCookies ("refreshToken",result.Data!.RefreshToken!,result.Data.RefreshTokenExpiration);
    return Ok (result);
   }
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
  /// <response code="200">Roles retrieved successfully</response>
  /// <response code="404">User not found</response>
  /// <response code="401">Unauthorized - authentication required</response>

  [HttpGet ("UserRoles/{username}")]
  [ProducesResponseType (typeof (ApiResponse<IList<string>>),StatusCodes.Status200OK)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status404NotFound)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status401Unauthorized)]
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
  /// <response code="200">Login successful, tokens provided</response>
  /// <response code="400">Validation failed - check required fields</response>

  [HttpPost ("AddRoleToUser")]
  [Consumes ("application/json")]
  [ProducesResponseType (typeof (ApiResponse<AuthDto>),StatusCodes.Status200OK)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status400BadRequest)]
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
  /// <response code="200">Login successful, tokens provided</response>
  /// <response code="400">Validation failed - check required fields</response>

  [HttpDelete ("RemoveRoleFromUser")]
  [Consumes ("application/json")]
  [ProducesResponseType (typeof (ApiResponse<AuthDto>),StatusCodes.Status200OK)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status400BadRequest)]
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
  /// <response code="200">Login successful, tokens provided</response>
  /// <response code="400">Validation failed - check required fields</response>

  [HttpPut ("ChangePassword")]
  [Consumes ("application/json")]
  [ProducesResponseType (typeof (ApiResponse<AuthDto>),StatusCodes.Status200OK)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status400BadRequest)]
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
  /// <response code="200">Login successful, tokens provided</response>
  /// <response code="400">Validation failed - check required fields</response>

  [HttpPut ("ActivateUserAccount/{username}")]
  [Consumes ("application/json")]
  [ProducesResponseType (typeof (ApiResponse<AuthDto>),StatusCodes.Status200OK)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status400BadRequest)]
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
  /// <response code="200">Login successful, tokens provided</response>
  /// <response code="400">Validation failed - check required fields</response>

  [HttpPut ("LockUserAccount/{username}")]
  [Consumes ("application/json")]
  [ProducesResponseType (typeof (ApiResponse<AuthDto>),StatusCodes.Status200OK)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status400BadRequest)]
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
  /// <response code="200">Login successful, tokens provided</response>
  /// <response code="400">Validation failed - check required fields</response>

  [HttpPut ("UnlockUserAccount/{username}")]
  [Consumes ("application/json")]
  [ProducesResponseType (typeof (ApiResponse<AuthDto>),StatusCodes.Status200OK)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status400BadRequest)]
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
  /// <response code="200">Login successful, tokens provided</response>
  /// <response code="400">Validation failed - check required fields</response>
  [HttpPut ("UpdateUserProfile")]
  [Consumes ("application/json")]
  [ProducesResponseType (typeof (ApiResponse<AuthDto>),StatusCodes.Status200OK)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status400BadRequest)]
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
  /// <response code="200">Login successful, tokens provided</response>
  /// <response code="400">Validation failed - check required fields</response>

  [HttpDelete ("DeleteUserAccount/{username}")]
  [Consumes ("application/json")]
  [ProducesResponseType (typeof (ApiResponse<AuthDto>),StatusCodes.Status200OK)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<ApiResponse<string>>> DeleteUserAccount ( string username )
  {
   ApiResponse<string> result = await authService.DeleteUserAccountAsync (username);
   return (!result.Success) ? BadRequest (result) : Ok (result);
  }

  /// <summary>
  /// Revokes an active refresh token to terminate a user session. 
  /// Extracts the token from the request body, or falls back to the HTTP cookies if not explicitly provided.
  /// </summary>
  /// <param name="revokeToken">The model containing the explicit token to revoke.</param>
  /// <returns>
  /// A success message confirming revocation, or an error response if validation fails or the session is already expired.
  /// </returns>
  /// <response code="200">Session successfully revoked</response>
  /// <response code="400">Validation failed - token is missing, invalid, or already expired</response>

  [HttpPost ("RevokeCurrentToken")]
  [Consumes ("application/json")]
  [ProducesResponseType (typeof (ApiResponse<AuthDto>),StatusCodes.Status200OK)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<ApiResponse<string>>> RevokeCurrentToken ( [FromBody] RevokeToken revokeToken )
  {
   var response = new ApiResponse<string> { Success=false,Language=localization.GetLanguage () };
   var token = revokeToken.Token??Request.Cookies ["refreshToken"];

   if (string.IsNullOrEmpty (token))
   {
    response.Message=localization.Get ("missingfields");
    return BadRequest (response);
   }

   var result = await jwtTokenService.RevokeTokenAsync (token);

   if (Request.Cookies ["refreshToken"]==token)
    RemoveRefreshTokenFromCookies ("refreshToken");

   return !result ? BadRequest ("Session is Expired!") : Ok ("session is revoked");

  }

  /// <summary>
  /// Refreshes the user's authentication and refresh tokens using a token stored in HTTP cookies.
  /// </summary>
  /// <returns>
  /// An <see cref="ApiResponse{AuthDto}"/> containing the new tokens upon success, 
  /// or an error message if the refresh token is missing or invalid.
  /// </returns>
  /// <response code="200">Login successful, tokens provided</response>
  /// <response code="400">Validation failed - check required fields (or refresh token missing/invalid)</response>

  [HttpGet ("refreshToken")]
  [Consumes ("application/json")]
  [ProducesResponseType (typeof (ApiResponse<AuthDto>),StatusCodes.Status200OK)]
  [ProducesResponseType (typeof (ApiResponse<object>),StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> RefreshToken ( )
  {
   var refreshToken = Request.Cookies ["refreshToken"];
   if (string.IsNullOrEmpty (refreshToken))
   {
    return BadRequest ("Refresh token is missing.");
   }
   var result = await jwtTokenService.RefreshTokenAsync (refreshToken);
   if (!result.IsAuthenticated)
   {
    return BadRequest (result.Message);
   }
   if (!string.IsNullOrEmpty (result.RefreshToken))
   {
    SetRefreshTokenInCookies ("accessToken",result.Token!,result.TokenExpiresIn);
    SetRefreshTokenInCookies ("refreshToken",result.RefreshToken,result.RefreshTokenExpiration);
   }
   return Ok (result);
  }

  /// <summary>
  /// Appends a secure, HttpOnly, and Strict SameSite cookie to the outgoing HTTP response.
  /// </summary>
  /// <param name="cookieKey">The name/key identifier of the cookie.</param>
  /// <param name="cookievalue">The token value or payload to store within the cookie.</param>
  /// <param name="cookieexpires">The universal expiration timestamp for the cookie.</param>
  private void SetRefreshTokenInCookies ( string cookieKey,string cookievalue,DateTime cookieexpires )
  {
   var cookieOptions = new CookieOptions
   {
    HttpOnly=true,
    SameSite=SameSiteMode.Strict,
    Secure=true,
    Expires=cookieexpires
   };
   Response.Cookies.Append (cookieKey,cookievalue,cookieOptions);
  }
  /// <summary>
  /// Removes a specified cookie from the client browser by expunging it from the response pipeline.
  /// </summary>
  /// <param name="cookieName">The name/key identifier of the cookie to safely remove.</param>
  private void RemoveRefreshTokenFromCookies ( string cookieName )
  {
   Response.Cookies.Delete (cookieName);
  }


 }
}
