using HR.Application.Dtos.AuthDtos;
using HR.Application.Dtos.RoleDtos;
using HR.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NuGet.Protocol;

namespace HRBackEndApi.Controllers
{
    [Route ( "api/[controller]" )]
    [ApiController]
    public class AuthUserController ( IAuthService authService ) : ControllerBase
    {
        /// <summary>
        /// Get all users in the system
        /// </summary>
        /// <returns>
        /// A list of all users in the system
        /// </returns>
        [HttpGet ( "GetAllUsers" )]
        public async Task<ActionResult<IList<string>>> GetAllUsers ( )
        {
            IList<string> result = await authService.GetAllUsersAsync ( );
            return Ok ( result );
        }
        /// <summary>
        /// Registers a new user in the system
        /// </summary>
        /// <param name="register"></param>
        /// <returns>
        /// The authentication result
        /// </returns>
        [HttpPost ( "RegisterUser" )]
        public async Task<ActionResult<AuthDto>> RegisterAsync ( [FromBody] RegisterDto register )
        {
            if ( !ModelState.IsValid )
                return BadRequest ( ModelState );

            AuthDto result = await authService.RegisterAsync ( register );

            if ( !result.IsAuthenticated )
                return BadRequest ( result.Message );

            return Ok ( result );
        }
        /// <summary>
        /// Logs in a user and returns an authentication token
        /// </summary>
        /// <param name="login"></param>
        /// <returns>
        /// The authentication token result
        /// </returns>
        [HttpPost ( "Login" )]
        public async Task<ActionResult<AuthDto>> Login ( [FromBody] LoginDto login )
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest ( ModelState );
            }
            var result = await authService.LoginAsync ( login );
            if ( !result.IsAuthenticated )
            {
                return BadRequest ( result.Message );
            }
            return Ok ( result );
        }
        /// <summary>
        /// Adds a role to a user
        /// </summary>
        /// <param name="addRoleToUser"></param>
        /// <returns>
        /// A message indicating the result of the operation
        /// </returns>
        [HttpPost ( "AddRoleToUser" )]
        public async Task<ActionResult<string>> AddRoleToUser ( [FromBody] AddRoleToUserDto addRoleToUser )
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest ( ModelState );
            }

            string result = await authService.AddRoleToUserAsync ( addRoleToUser );

            return Ok ( result );
        }
      
        /// <summary>
        /// Gets a list of roles assigned to a specific user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        /// A list of roles assigned to the specified user
        /// </returns>
        [HttpGet ( "GetUserRoles/{username}" )]
        public async Task<ActionResult<UserRolesDto>> GetUserRoles ( string username )
        {
            if ( string.IsNullOrEmpty ( username ) )
            {
                return BadRequest ( "Username is required" );
            }
            UserRolesDto result = await authService.GetUserRolesAsync ( username );
            return Ok ( result );
        }
        /// <summary>
        /// Changes the password for a user
        /// </summary>
        /// <param name="changePasswordDto"></param>
        /// <returns>
        /// A message indicating the result of the operation
        /// </returns>
        [HttpPut ( "ChangePassword" )]
        public async Task<ActionResult<string>> ChangePassword ( [FromBody] ChangePasswordDto changePasswordDto )
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest ( ModelState );
            }
            string result = await authService.ChangePasswordAsync ( changePasswordDto );
            return Ok ( result );
        }
        /// <summary>
        /// Activates or deactivates a user account
        /// </summary>
        /// <param name="username"></param>
        /// <param name="isActivated"></param>
        /// <returns>
        /// A message indicating the result of the operation
        /// </returns>
        [HttpPut ( "ActivateUserAccount/{username}" )]
        public async Task<ActionResult<string>> ActivateUserAccount ( string username, bool isActivated )
        {
            if ( string.IsNullOrEmpty ( username ) || isActivated == null )
            {
                return BadRequest ( "Username and activation status are required" );
            }
            string result = await authService.ActivateUserAccountAsync ( username, isActivated );
            return Ok ( result );
        }
        /// <summary>
        /// Locks a user account for a specified duration
        /// </summary>
        /// <param name="username"></param>
        /// <param name="lockoutDuration"></param>
        /// <returns>
        /// A message indicating the result of the operation
        /// </returns>
        [HttpPut ( "LockUserAccount/{username}" )]
        public async Task<ActionResult<string>> LockUserAccount ( string username, TimeSpan lockoutDuration )
        {
            if ( string.IsNullOrEmpty ( username ) || lockoutDuration == null )
            {
                return BadRequest ( "Username and lockout duration are required" );
            }
            string result = await authService.LockUserAccountAsync ( username, lockoutDuration );
            return Ok ( result );
        }
        /// <summary>
        /// Unlocks a user account
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        /// A message indicating the result of the operation
        /// </returns>
        [HttpPut ( "UnlockUserAccount/{username}" )]
        public async Task<ActionResult<string>> UnlockUserAccount ( string username )
        {
            if ( string.IsNullOrEmpty ( username ) )
            {
                return BadRequest ( "Username is required" );
            }
            string result = await authService.UnlockUserAccountAsync ( username );
            return Ok ( result );
        }
        /// <summary>
        /// Updates the profile information of a user
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns>
        /// The updated user profile
        /// </returns>
        [HttpPut ( "UpdateUserProfile" )]
        public async Task<ActionResult<UserProfile>> UpdateUserProfile ( [FromBody] UserProfile userProfile )
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest ( ModelState );
            }
            UserProfile result = await authService.UpdateUserProfileAsync ( userProfile );
            return Ok ( result );
        }
        /// <summary>
        /// Deletes a user account from the system
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        /// A message indicating the result of the operation
        /// </returns>
        [HttpDelete ( "DeleteUserAccount/{username}" )]
        public async Task<ActionResult<string>> DeleteUserAccount ( string username )
        {
            if ( string.IsNullOrEmpty ( username ) )
            {
                return BadRequest ( "Username is required" );
            }
            string result = await authService.DeleteUserAccountAsync ( username );
            return Ok ( result );

        }
        /// <summary>
        /// Removes a role from a user
        /// </summary>
        /// <param name="removeRoleFromUser"></param>
        /// <returns>
        /// A message indicating the result of the operation
        /// </returns>
        [HttpDelete ( "RemoveRoleFromUser" )]
        public async Task<ActionResult<string>> RemoveRoleFromUser ( [FromBody] AddRoleToUserDto removeRoleFromUser )
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest ( ModelState );
            }
            string result = await authService.RemoveUserRoleAsync ( removeRoleFromUser );
            return Ok ( result );
        }

    }
}