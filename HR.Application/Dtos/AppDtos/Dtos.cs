using HR.Domain.Models.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HR.Application.Dtos.AppDtos
{
 //Authentication (Registeration and Login)
 //=========================================
 /// <summary>
 /// Minimal user information returned by authentication endpoints.
 /// </summary>
 /// <remarks>
 /// Contains identifying information about the user that is safe to return in authentication responses.
 /// </remarks>
 public class UserRequest
 {
  /// <summary>The unique identifier of the user.</summary>
  public Guid UserId { get; set; } = Guid.NewGuid ();
  /// <summary>The username/login identifier.</summary>
  public string? UserName { get; set; }
  /// <summary>User first name.</summary>
  public string? FirstName { get; set; }
  /// <summary>User last name.</summary>
  public string? LastName { get; set; }
  /// <summary>Concatenation of first and last names when available.</summary>
  public string? FullName { get; set; }
  /// <summary>User email address.</summary>
  public string? Email { get; set; }
  /// <summary>Whether the account is currently active.</summary>
  public bool IsActivatedAccount { get; set; } = true;
  /// <summary>Timestamp when the user profile was created.</summary>
  public DateTime CreationTime { get; set; }
 }
 /// <summary>
 /// Authentication response returned after register/login operations.
 /// </summary>
 public class AuthDto
 {
  /// <summary>User summary information.</summary>
  public UserRequest? User { get; set; }
  /// <summary>List of role names assigned to the user.</summary>
  public List<string>? Roles { get; set; }
  /// <summary>JWT or similar access token.</summary>
  public string? Token { get; set; }
  /// <summary>Expiration date/time of the access token.</summary>
  public DateTime TokenExpiresIn { get; set; }
  /// <summary>Whether authentication succeeded.</summary>
  public bool IsAuthenticated { get; set; } = false;
  /// <summary>Whether the account is activated.</summary>
  public bool ISActivatedAccount { get; set; } = true;
  /// <summary>Optional message about the authentication result.</summary>
  public string? Message { get; set; }
  [JsonIgnore]
  /// <summary>Refresh token used to obtain new access tokens. Not serialized in responses.</summary>
  public string? RefreshToken { get; set; }
  [JsonIgnore]
  /// <summary>Expiration time for the refresh token. Not serialized in responses.</summary>
  public DateTime RefreshTokenExpiration { get; set; }
 }
 /// <summary>
 /// Credentials used to sign in a user.
 /// </summary>
 public class LoginDto
 {
  /// <summary>User login name.</summary>
  [Required] public required string Username { get; set; }
  /// <summary>User password.</summary>
  [Required] public required string Password { get; set; }
 }
 /// <example>
 /// {
 ///  "Username": "jdoe",
 ///  "Password": "P@ssw0rd"
 /// }
 /// </example>
 /// <summary>
 /// Data required to create a new user account.
 /// </summary>
 public class RegisterDto
 {
  [Required, MaxLength (150)] public required string Username { get; set; }
  [Required, MaxLength (150)] public required string FirstName { get; set; }
  [Required, MaxLength (150)] public required string LastName { get; set; }
  [Required, MaxLength (150)] public required string Email { get; set; }
  [Required, MaxLength (80)] public required string Password { get; set; }
  [Required, StringLength (11)] public string? PhoneNumber { get; set; }
  /// <summary>Optional profile image.</summary>
  public byte [ ]? ProfilePicture { get; set; }
  /// <summary>Whether the account should start activated.</summary>
  public bool IsActivatedAccount { get; set; } = true;
  /// <summary>Account creation date.</summary>
  public DateTime CreatedOn = DateTime.Today;
 }
 /// <example>
 /// {
 ///  "Username": "jdoe",
 ///  "FirstName": "John",
 ///  "LastName": "Doe",
 ///  "Email": "john@example.com",
 ///  "Password": "P@ssw0rd"
 /// }
 /// </example>
 /// <summary>
 /// Request to change a user's password.
 /// </summary>
 public class ChangePasswordDto
 {
  /// <summary>The username for which the password will be changed.</summary>
  [Required] public string? Username { get; set; }
  /// <summary>Current password.</summary>
  [Required] public string? OldPassword { get; set; }
  /// <summary>New desired password.</summary>
  [Required] public string? NewPassword { get; set; }
  /// <summary>Confirmation of the new password.</summary>
  [Required] public string? ConfirmPassword { get; set; }
 }
 /// <example>
 /// {
 ///  "Username": "jdoe",
 ///  "OldPassword": "OldPass123",
 ///  "NewPassword": "NewPass123",
 ///  "ConfirmPassword": "NewPass123"
 /// }
 /// </example>

 /// <summary>
 /// Public user profile information returned by profile endpoints.
 /// </summary>
 public record UserProfile ( string FirstName,string LastName,string Email,string NormalizedEmail,string UserName,string NormalizedUserName,
  string PhoneNumber,byte [ ]? ProfilePicture );

    //Roles 
    //=====
    /// <summary>Role representation used in role management endpoints.</summary>
    public class RoleDto
    {
        public int RoleId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
 /// <summary>DTO to assign or remove a role to/from a user.</summary>
 public record AssignRoleDto ( string UserName,string RoleName );
 /// <summary>DTO for creating or updating a role.</summary>
 public record CreateRoleDto ( string Name,string Description );
 /// <example>
 /// { "Name": "Editor", "Description": "Can edit content" }
 /// </example>

 //Permission
 //===========
 /// <summary>Permission DTO used by permission endpoints.</summary>
 public record PermissionDto ( int PermissionId,string PermissionName );
 /// <summary>Request to create a new permission.</summary>
 public class CreatePermissionDto { [Required] public string? PermissionName { get; set; } };
 /// <example>
 /// { "PermissionName": "CanViewReports" }
 /// </example>

 //Modules
 //========

 /// <summary>Represents a top-level module in the system.</summary>
 public record ModuleDto ( int ModuleId,string ModuleName_en,string ModuleName_ar,byte [ ]? ModuleImage );
 /// <summary>Request to create a new module.</summary>
 public record CreateModuleDto ( string ModuleName_en,string ModuleName_ar,byte [ ]? ModuleImage );
 /// <example>
 /// { "ModuleName_en": "Payroll", "ModuleName_ar": "الرواتب" }
 /// </example>

 //SubModules
 //===========
 /// <summary>Represents a sub-module used within a parent module.</summary>
 public record SubModuleDto ( int SubModuleId,string SubModuleName_en,string SubModuleName_ar );
 /// <summary>Request to create a submodule.</summary>
 public record CreateSubModuleDto ( string SubModuleName_en,string SubModuleName_ar );
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


  /// <summary>Page identifier.</summary>
  //public int? PageId { get; set; } = mp.PageId;
  ///// <summary>Page name in English.</summary>
  //public string PageName_en { get; set; } = mp.PageName_en;
  ///// <summary>Page name in Arabic.</summary>
  //public string PageName_ar { get; set; } = mp.PageName_ar;
  ///// <summary>Page route/URL.</summary>
  //public string PageUrl { get; set; } = mp.PageUrl;
  public List<SubModulePagesDto>? ListModulePagesDto { get; set; } = [ ];
 }
 /// <summary>DTO for submodule page CRUD operations.</summary>
 public record SubModulePagesDto ( int SubModuleId,int PageId,string PageName_en,string PageName_ar,string PageUrl );

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
  [Required]
  public string? SubModuleName { get; set; } = language=="ar" ? msm.SubModule!.SubModuleName_ar : msm.SubModule!.SubModuleName_en;

 }
 /// <summary>Request to assign or remove a submodule to/from a module.</summary>
 public record Assign_RemoveModule_SubModuleDto ( int ModuleId,int SubModuleId );
 /// <example>
 /// { "ModuleId": 5, "SubModuleId": 12 }
 /// </example>

 //RoleSubModules
 //===============

 /// <summary>DTO used to represent submodules attached to a role with localized names.</summary>
 public class GetRoleSubModules ( Role_SubModules rms,string language )
 {
  /// <summary>Role identifier as string.</summary>
  public string? RoleId { get; set; }
  /// <summary>Role display name.</summary>
  public string? RoleName { get; set; } = rms.Role!.Name;
  /// <summary>Module identifier.</summary>
  //public int ModuleID = rms.ModuleId;
  ///// <summary>Localized module name.</summary>
  //public string ModuleName { get; set; } = language=="en" ? rms.Module!.ModuleName_en : rms.Module!.ModuleName_ar;
  ///// <summary>Submodule identifier.</summary>
  //public int subModuleID = rms.SubModuleId;
  ///// <summary>Localized submodule name.</summary>
  //public string? SubModuleName { get; set; } = language=="en" ? rms.SubModule!.SubModuleName_en : rms.SubModule!.SubModuleName_ar;

  public GetModule_SubModulesDto? GetModule_SubModulesDto { get; set; }


 }
 /// <summary>Request to assign or remove a submodule to/from a role.</summary>
 public record Assign_Remove_SubModuleToRole ( string RoleId,int ModuleId,int SubModuleId );
 /// <example>
 /// { "RoleId": 3, "ModuleId": 1, "SubModuleId": 5 }
 /// </example>

 //RoleSubModulePages
 //===================

 //public class GetRoleSubModulePages ( Role_SubModulePages rsp,string language )
 //{
 // /// <summary>Role identifier as string.</summary>
 // public string? RoleId { get; set; }
 // /// <summary>Role display name.</summary>
 // public string? RoleName { get; set; } = rsp.Role!.Name;
 // /// <summary>subModule identifier.</summary>
 // public int SubModuleId { get; set; }
 // /// <summary>Localized submodule name.</summary>
 // public string? SubModuleName { get; set; } = language=="en" ? rsp.SubModulePage!.SubModule!.SubModuleName_en : rsp.SubModulePage!.SubModule!.SubModuleName_ar;

 // ///// <summary>Page details</summary>
 // public List<SubModulePagesDto>? ListSubModulePagesdto { get; set; } = [ ];
 //}
 public record Assign_Remove_PageToRole ( string RoleId,int PageId );


}
