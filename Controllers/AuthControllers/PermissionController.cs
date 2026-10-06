using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Helpers;
using HR.Application.Interfaces;
using HR.Application.Response;
using HR.Domain.Models.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRBackEndApi.Controllers.AuthControllers
{

 [Route ("api/[controller]")]
 [ApiController]
 public class PermissionController ( IBaseService<Permission> PermissionService,IMapper mapper,ILocalizationService localization ) : ControllerBase
 {

  /// <summary>
  /// Get All Permissions in the system.
  /// </summary>
  /// <returns>
  /// 
  /// </returns>
  [HttpGet ("GetAllPermissions")]
  public async Task<ActionResult<ApiResponse<IEnumerable<PermissionDto>>>> GetAllPermissionsAsync ( )
  {
   ApiResponse<IEnumerable<Permission>> permissions = await PermissionService.GetAllAsync ();
   if (!permissions.Success)
    return NotFound (permissions);

   IEnumerable<PermissionDto> getPermissions = mapper.Map<IEnumerable<PermissionDto>> (permissions.Data);
   return Ok (new ApiResponse<IEnumerable<PermissionDto>>
   {
    Success=true,
    Message=permissions.Message,
    Data= [.. getPermissions.OrderBy (p => p.PermissionName)],
    Language=permissions.Language
   });
  }
  /// <summary>
  /// Get Permission Id for a specific Permission Name
  /// </summary>
  /// <param name="permissionName"></param>
  /// <returns>
  /// returns PermissionDto with PermissionId and Permission Name
  /// </returns>
  /// 
  [HttpGet ("GetPermissionByName/{permissionName}")]
  public async Task<ActionResult<ApiResponse<IEnumerable<PermissionDto>>>> GetPermissionIdAsync ( string permissionName )
  {
   if (string.IsNullOrWhiteSpace (permissionName))
    return BadRequest (new ApiResponse<IEnumerable<PermissionDto>>
    {
     Success=false,
     Message=localization.Get ("missingfields"),
     Data=null,
     Language=localization.GetLanguage ()
    });
   ApiResponse<Permission> permission = await PermissionService.FindItemAsync (p => p.PermissionName==permissionName);
   return (!permission.Success) ? NotFound (permission) : Ok (new ApiResponse<PermissionDto>
   {
    Success=permission.Success,
    Message=permission.Message,
    Data=mapper.Map<PermissionDto> (permission.Data),
    Language=permission.Language
   });
  }
  /// <summary>
  /// Create a new Permission by Permission Name
  /// </summary>
  /// <param name="createPermission"></param>
  /// <returns>
  /// get all permissions ordered by name
  /// </returns>
  [HttpPost ("AddNewPermission")]
  public async Task<ActionResult<ApiResponse<IEnumerable<PermissionDto>>>> AddPermissionAsync ( [FromBody] CreatePermissionDto createPermission )
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

   createPermission.PermissionName=char.ToUpper (createPermission.PermissionName [0])+createPermission.PermissionName [1..];
   ApiResponse<bool> isExist = await PermissionService.IsExistAsync (p => p.PermissionName==createPermission.PermissionName);
   if (isExist.Success)
    return BadRequest (new ApiResponse<object>
    {
     Success=false,
     Message=isExist.Message,
     Data=null,
     Language=isExist.Language
    });

   ApiResponse<IEnumerable<Permission>> permission = await PermissionService.CreateAsyncAndGetAll (mapper.Map<Permission> (createPermission),
    p => p.PermissionName==createPermission.PermissionName,getAllCriteria: null);
   return (!permission.Success) ? BadRequest (permission) : Created ("",new ApiResponse<object>
   {
    Success=permission.Success,
    Message=permission.Message,
    Data=(mapper.Map<IEnumerable<PermissionDto>> (permission.Data)).OrderBy (p => p.PermissionName),
    Language=permission.Language
   });
  }
  /// <summary>
  /// Update Permission by name and Id
  /// </summary>
  /// <param name="permissionDto"></param>
  /// <returns>
  /// the updated permission
  /// </returns>
  [HttpPut ("UpdatePermission")]
  public async Task<ActionResult<ApiResponse<PermissionDto>>> UpdatePermissionAsync ( [FromBody] PermissionDto permissionDto )
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
   var item = await PermissionService.FindItemAsync (p => p.PermissionName==permissionDto.PermissionName);
   if (!item.Success) return NotFound (item);
   ApiResponse<Permission> updatedPermission = await PermissionService.UpdateAsync (mapper.Map<Permission> (permissionDto));
   return (!updatedPermission.Success) ? BadRequest (updatedPermission) : Ok (new ApiResponse<object>
   {
    Success=updatedPermission.Success,
    Message=updatedPermission.Message,
    Data=mapper.Map<PermissionDto> (updatedPermission.Data),
    Language=updatedPermission.Language
   });
  }
  /// <summary>
  /// Delete permission by name
  /// </summary>
  /// <param name="permissionName"></param>
  /// <returns></returns>
  [HttpDelete ("DeletePermission/{permissionName}")]
  public async Task<ActionResult<ApiResponse<string>>> DeletePermissionAsync ( string permissionName )
  {
   ApiResponse<string> response = new ()
   {
    Success=false,
    Data=null,
    Language=localization.GetLanguage ()
   };

   if (string.IsNullOrEmpty (permissionName))
   {
    response.Message=localization.Get ("missingfields");
    return BadRequest (response);
   }
   ApiResponse<int> affectedRowsCount = await PermissionService.DeleteAsync (p => p.PermissionName==permissionName);
   return (!affectedRowsCount.Success) ? BadRequest (affectedRowsCount) : Ok (affectedRowsCount);
  }
  /// <summary>
  /// delete a group of permissions together and permission names should be separated by comma
  /// </summary>
  /// <param name="permissionNames"></param>
  /// <returns>
  /// the number of deleted permissions
  /// </returns>
  [HttpDelete ("DeleteBulkOfPermissions/{permissionNames}")]
  public async Task<ActionResult<ApiResponse<int>>> DeleteBulkPermissionsAsync ( string permissionNames )
  {
   string [ ] deletedpermissions = permissionNames.Split (',');
   int count = deletedpermissions.Length;
   int deletedrowsAffected = 0;
   foreach (var permissionName in deletedpermissions)
   {
    ApiResponse<Permission> deleteditem = await PermissionService.FindItemAsync (p => p.PermissionName==permissionName);
    if (!deleteditem.Success) return BadRequest (deleteditem);
    ApiResponse<int> rowsaffected = await PermissionService.DeleteAsync (deleteditem.Data!);
    deletedrowsAffected+=rowsaffected.Data;
   }
   return (deletedrowsAffected!=count) ? BadRequest (new ApiResponse<int>
   {
    Success=false,
    Message=localization.Get ("notalldeleted"),
    Data=deletedrowsAffected,
    Language=localization.GetLanguage ()
   }) :
   Ok
   (new ApiResponse<int>
   {
    Success=true,
    Message=$"{deletedrowsAffected} {localization.Get ("itemsdeleted")} ",
    Data=deletedrowsAffected,
    Language=localization.GetLanguage ()
   });
  }

 }
}
