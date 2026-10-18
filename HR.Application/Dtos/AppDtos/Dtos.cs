using HR.Domain.Models.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HR.Application.Dtos.AppDtos
{
 // Authentication (Registration and Login)
 // =========================================
 /// <summary>
 /// Minimal user information returned by authentication endpoints.
 /// </summary>
 /// <example>
 /// {
 ///   "UserId": "00000000-0000-0000-0000-000000000000",
 ///   "UserName": "jdoe",
 ///   "FirstName": "John",
 ///   "LastName": "Doe",
 ///   "FullName": "John Doe",
 ///   "Email": "john@example.com",
 ///   "IsActivatedAccount": true,
 ///   "CreationTime": "2024-01-01T00:00:00Z"
 /// }
 /// </example>
 public class UserRequest
 {
  /// <summary>The unique identifier of the user.</summary>
  public Guid UserId { get; set; } = Guid.NewGuid ();
  /// <summary>The username/login identifier.</summary>
  public string UserName { get; set; } = string.Empty;
  /// <summary>User first name.</summary>
  public string FirstName { get; set; } = string.Empty;
  /// <summary>User last name.</summary>
  public string LastName { get; set; } = string.Empty;
  /// <summary>Concatenation of first and last names when available.</summary>
  public string FullName { get; set; } = string.Empty;
  /// <summary>User email address.</summary>
  public string Email { get; set; } = string.Empty;
  /// <summary>Whether the account is currently active.</summary>
  public bool IsActivatedAccount { get; set; } = true;
  /// <summary>Timestamp when the user profile was created.</summary>
  public DateTime CreationTime { get; set; }
 }

 /// <summary>
 /// Authentication response returned after register/login operations.
 /// </summary>
 /// <example>
 /// {
 ///   "User": { "UserName": "jdoe", "Email": "john@example.com" },
 ///   "Roles": ["Admin","User"],
 ///   "Token": "eyJhbGci...",
 ///   "TokenExpiresIn": "2024-01-01T01:00:00Z",
 ///   "IsAuthenticated": true,
 ///   "Message": "Login successful"
 /// }
 /// </example>
 public class AuthDto
 {
  /// <summary>User summary information.</summary>
  public UserRequest User { get; set; } = new UserRequest ();
  /// <summary>List of role names assigned to the user.</summary>
  public List<string> Roles { get; set; } = new List<string> ();
  /// <summary>JWT or similar access token.</summary>
  public string Token { get; set; } = string.Empty;
  /// <summary>Expiration date/time of the access token.</summary>
  public DateTime TokenExpiresIn { get; set; }
  /// <summary>Whether authentication succeeded.</summary>
  public bool IsAuthenticated { get; set; } = false;
  /// <summary>Whether the account is activated.</summary>
  public bool ISActivatedAccount { get; set; } = true;
  /// <summary>Optional message about the authentication result.</summary>
  public string? Message { get; set; }

  /// <summary>Refresh token used to obtain new access tokens. Not serialized in responses.</summary>
  [JsonIgnore]
  public string? RefreshToken { get; set; }

  /// <summary>Expiration time for the refresh token. Not serialized in responses.</summary>
  [JsonIgnore]
  public DateTime RefreshTokenExpiration { get; set; }
 }

 /// <summary>
 /// Credentials used to sign in a user.
 /// </summary>
 /// <example>
 /// { "Username": "jdoe", "Password": "P@ssw0rd" }
 /// </example>
 public class LoginDto
 {
  /// <summary>User login name.</summary>
  [Required] public string Username { get; set; } = string.Empty;
  /// <summary>User password.</summary>
  [Required] public string Password { get; set; } = string.Empty;
 }

 /// <summary>
 /// Data required to create a new user account.
 /// </summary>
 /// <example>
 /// {
 ///   "Username": "jdoe",
 ///   "FirstName": "John",
 ///   "LastName": "Doe",
 ///   "Email": "john@example.com",
 ///   "Password": "P@ssw0rd",
 ///   "PhoneNumber": "01234567890"
 /// }
 /// </example>
 public class RegisterDto
 {
  [Required, MaxLength (150)] public string Username { get; set; } = string.Empty;
  [Required, MaxLength (150)] public string FirstName { get; set; } = string.Empty;
  [Required, MaxLength (150)] public string LastName { get; set; } = string.Empty;
  [Required, MaxLength (150)] public string Email { get; set; } = string.Empty;
  [Required, MaxLength (80)] public string Password { get; set; } = string.Empty;
  [Required, StringLength (11)] public string? PhoneNumber { get; set; }
  /// <summary>Optional profile image as byte array.</summary>
  public byte [ ]? ProfilePicture { get; set; }
  /// <summary>Whether the account should start activated.</summary>
  public bool IsActivatedAccount { get; set; } = true;
  /// <summary>Account creation date.</summary>
  public DateTime CreatedOn { get; set; } = DateTime.Today;
 }

 /// <summary>
 /// Request to change a user's password.
 /// </summary>
 /// <example>
 /// { "Username": "jdoe", "OldPassword": "OldPass123", "NewPassword": "NewPass123", "ConfirmPassword": "NewPass123" }
 /// </example>
 public class ChangePasswordDto
 {
  /// <summary>The username for which the password will be changed.</summary>
  [Required] public string Username { get; set; } = string.Empty;
  /// <summary>Current password.</summary>
  [Required] public string OldPassword { get; set; } = string.Empty;
  /// <summary>New desired password.</summary>
  [Required] public string NewPassword { get; set; } = string.Empty;
  /// <summary>Confirmation of the new password.</summary>
  [Required] public string ConfirmPassword { get; set; } = string.Empty;
 }

 /// <summary>
 /// Public user profile information returned by profile endpoints.
 /// </summary>
 public class UserProfile
 {
  /// <summary>User first name.</summary>
  public string FirstName { get; set; } = string.Empty;
  /// <summary>User last name.</summary>
  public string LastName { get; set; } = string.Empty;
  /// <summary>User email address.</summary>
  public string Email { get; set; } = string.Empty;
  /// <summary>Normalized email for lookups.</summary>
  public string NormalizedEmail { get; set; } = string.Empty;
  /// <summary>Username.</summary>
  public string UserName { get; set; } = string.Empty;
  /// <summary>Normalized username for lookups.</summary>
  public string NormalizedUserName { get; set; } = string.Empty;
  /// <summary>Contact phone number.</summary>
  public string PhoneNumber { get; set; } = string.Empty;
  /// <summary>Optional profile image as byte array.</summary>
  public byte [ ]? ProfilePicture { get; set; }
 }

 // Roles
 // =====
 /// <summary>Role representation used in role management endpoints.</summary>
 /// <example>
 /// { "RoleId": "role-id", "Name": "Admin", "Description": "Administrator" }
 /// </example>
 public class RoleDto
 {
  /// <summary>Role identifier.</summary>
  public string Id { get; set; } = string.Empty;
  /// <summary>Role name.</summary>
  public string Name { get; set; } = string.Empty;
  /// <summary>Optional description for the role.</summary>
  public string Description { get; set; } = string.Empty;
 }

 /// <summary>DTO to assign or remove a role to/from a user.</summary>
 /// <example>
 /// { "UserName": "jdoe", "RoleName": "Editor" }
 /// </example>
 public class AssignRoleDto
 {
  [Required] public string UserName { get; set; } = string.Empty;
  [Required] public string RoleName { get; set; } = string.Empty;
 }

 /// <summary>DTO for creating or updating a role.</summary>
 /// <example>
 /// { "Name": "Editor", "Description": "Can edit content" }
 /// </example>
 public class CreateRoleDto
 {
  [Required] public string Name { get; set; } = string.Empty;
  [Required] public string Description { get; set; } = string.Empty;
 }

 // Permission
 // ==========
 /// <summary>Permission DTO used by permission endpoints.</summary>
 public class PermissionDto
 {
  /// <summary>Permission identifier.</summary>
  public int PermissionId { get; set; }
  [Required] public string PermissionName { get; set; } = string.Empty;
 }
 /// <summary>Request to create a new permission.</summary>
 public class CreatePermissionDto
 {
  [Required] public string PermissionName { get; set; } = string.Empty;
 }
 /// <example>
 /// { "PermissionName": "CanViewReports" }
 /// </example>

 //Modules
 //========

 /// <summary>Represents a top-level module in the system.</summary>
 public class ModuleDto
 {
  public int ModuleId { get; set; }
  public string ModuleName_en { get; set; } = string.Empty;
  public string ModuleName_ar { get; set; } = string.Empty;
  public byte [ ] ModuleImage { get; set; } = [ ];
 }
 /// <summary>Request to create a new module.</summary>
 public class CreateModuleDto
 {
  public string ModuleName_en { get; set; } = string.Empty;
  public string ModuleName_ar { get; set; } = string.Empty;
  public byte [ ] ModuleImage { get; set; } = [ ];
 }
 /// <example>
 /// { "ModuleName_en": "Payroll", "ModuleName_ar": "الرواتب" }
 /// </example>

 //SubModules
 //===========
 /// <summary>Represents a sub-module used within a parent module.</summary>
 public class SubModuleDto
 {
  public int SubModuleId { get; set; }
  public string SubModuleName_en { get; set; } = string.Empty;
  public string SubModuleName_ar { get; set; } = string.Empty;
 }
 /// <summary>Request to create a submodule.</summary>
 public class CreateSubModuleDto
 {
  public string SubModuleName_en { get; set; } = string.Empty;
  public string SubModuleName_ar { get; set; } = string.Empty;
 }
 /// <example>
 /// { "SubModuleName_en": "EmployeeOnboarding", "SubModuleName_ar": "تعيين الموظف" }
 /// </example>

 //SubModulePages
 //===============
 /// <summary>DTO representing a submodule page along with its parent submodule's localized name.</summary>
 public class SubModulePagesDtowithModule ( SubModulePages mp,string language )
 {
  /// <summary>The parent submodule identifier.</summary> 
  public int SubModuleId { get; set; } = mp.SubModuleId;
  /// <summary>Localized submodule name.</summary>
  [Required]
  public string? SubModuleName { get; set; } = language=="en" ? mp.SubModule!.SubModuleName_en : mp.SubModule!.SubModuleName_ar;
  public List<SubModulePagesDto>? ListModulePagesDto { get; set; } = [ ];
 }
 /// <summary>DTO for submodule page CRUD operations.</summary>
 public class SubModulePagesDto
 {
  public int SubModuleId { get; set; }
  public int PageId { get; set; }
  public string PageName_en { get; set; } = string.Empty;
  public string PageName_ar { get; set; } = string.Empty;
  public string PageUrl { get; set; } = string.Empty;
 }
 //ModuleSubModules
 //=================
 /// <summary>DTO used to represent submodules attached to a module with localized names.</summary>
 public class GetModule_SubModulesDto ( Module_SubModules msm,string language )
 {
  /// <summary>Parent module id.</summary>
  public int ModuleId { get; set; } = msm.ModuleId;
  /// <summary>Localized module name.</summary>
  public string ModuleName { get; set; } = language=="ar" ? msm.Module!.ModuleName_ar : msm.Module!.ModuleName_en;
  /// <summary>Submodule id.</summary>
  public int SubModuleId { get; set; } = msm.SubModuleId;
  /// <summary>Localized submodule name.</summary>

  public string SubModuleName { get; set; } = language=="ar" ? msm.SubModule!.SubModuleName_ar! : msm.SubModule!.SubModuleName_en!;

 }
 /// <summary>Request to assign or remove a submodule to/from a module.</summary>
 public class Assign_RemoveModule_SubModuleDto
 {
  [Required] public int ModuleId { get; set; }
  [Required] public int SubModuleId { get; set; }
 }
 /// <summary>
 /// <example>
 /// { "ModuleId": 5, "SubModuleId": 12 }
 /// </example>
 /// </summary>

 //RoleSubModules
 //===============

 /// <summary>DTO used to represent submodules attached to a role with localized names.</summary>
 public class GetRoleSubModules ( Role_SubModules rms,string language )
 {
  /// <summary>Role identifier as string.</summary>
  public string RoleId { get; set; } = rms.Role!.Id;
  /// <summary>Role display name.</summary>
  public string RoleName { get; set; } = rms.Role!.Name!;
  public GetModule_SubModulesDto? GetModule_SubModulesDto { get; set; }
 }
 /// <summary>Request to assign or remove a submodule to/from a role.</summary>
 public class Assign_Remove_SubModuleToRole
 {
  [Required] public required string RoleId { get; set; }
  [Required] public required int ModuleId { get; set; }
  [Required] public required int SubModuleId { get; set; }
 }
 /// <example>
 /// { "RoleId": 3, "ModuleId": 1, "SubModuleId": 5 }
 /// </example>



}
