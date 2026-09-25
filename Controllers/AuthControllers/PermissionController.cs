using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Helpers;
using HR.Application.Interfaces;
using HR.Domain.Models.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRBackEndApi.Controllers.AuthControllers
{

 [Route ( "api/[controller]" )]
 [ApiController]
 public class PermissionController ( IBaseService<Permission> PermissionService, IMapper mapper ) : ControllerBase
 {
  /// <summary>
  /// Get All Permissions in the system.
  /// </summary>
  /// <returns>
  /// 
  /// </returns>
  [HttpGet ( "ReadAllPermissions" )]
  public async Task<ActionResult<IEnumerable<PermissionDto>>> GetAllPermissionsAsync ( )
  {
   var (permissions, isSuccess) = await PermissionService.GetAllAsync ( );
   if ( !isSuccess )
    return NotFound ( "No items found." );

   IEnumerable<PermissionDto> getPermissions = mapper.Map<IEnumerable<PermissionDto>> ( permissions );
   return Ok ( getPermissions.OrderBy ( p => p.PermissionName ).ToList ( ) );
  }
  /// <summary>
  /// Get Permission Id for a specific Permission Name
  /// </summary>
  /// <param name="permissionName"></param>
  /// <returns>
  /// returns PermissionDto with PermissionId and Permission Name
  /// </returns>
  /// 
  [HttpGet ( "ReadPermissionId/{permissionName}" )]
  public async Task<ActionResult<PermissionDto>> GetPermissionIdAsync ( string permissionName )
  {
   if ( string.IsNullOrWhiteSpace ( permissionName ) )
    return BadRequest ( "permission Name is required." );
   var (permission, isSuccess) = await PermissionService.GetByConditionAsync ( p => p.PermissionName == permissionName );
   if ( !isSuccess )
    return NotFound ( $"this permission {permissionName} is not found " );
   return Ok ( permission );
  }
  /// <summary>
  /// Create a new Permission by Permission Name
  /// </summary>
  /// <param name="createPermission"></param>
  /// <returns>
  /// get all permissions ordered by name
  /// </returns>
  [HttpPost ( "AddNewPermission" )]
  public async Task<ActionResult<IEnumerable<PermissionDto>>> AddPermissionAsync ( [FromBody] CreatePermissionDto createPermission )
  {
   if ( string.IsNullOrWhiteSpace ( createPermission.PermissionName ) ) return BadRequest ( "permission Name is required." );

   createPermission.PermissionName = char.ToUpper ( createPermission.PermissionName [ 0 ] ) + createPermission.PermissionName [ 1.. ];
   bool isExist = await PermissionService.IsExistAsync ( p => p.PermissionName == createPermission.PermissionName, HttpRequestType.Post );
   if ( isExist )
    return BadRequest ( $"this permission {createPermission.PermissionName} is already exists." );

   var (permission, isSuccess) = await PermissionService.CreateAsync ( mapper.Map<Permission> ( createPermission ) );
   if ( !isSuccess )
    return BadRequest ( "Error in creation of permission" );

   return await GetAllPermissionsAsync ( );
  }
  /// <summary>
  /// Update Permission by name and Id
  /// </summary>
  /// <param name="permissionDto"></param>
  /// <returns>
  /// the updated permission
  /// </returns>
  [HttpPut ( "UpdatePermission" )]
  public async Task<ActionResult<PermissionDto>> UpdatePermissionAsync ( [FromBody] PermissionDto permissionDto )
  {
   if ( string.IsNullOrWhiteSpace ( permissionDto.PermissionName ) || permissionDto.PermissionId is null )
   {
    return BadRequest ( "the missing fields are required" );
   }
   var (checkPermission, isExist) = await PermissionService.FindAsync ( p => p.PermissionName == permissionDto.PermissionName );
   if ( isExist )
    return BadRequest ( $"this item {permissionDto.PermissionName} already exists." );
   var (updatedPerm, isSuccess) = await PermissionService.UpdateAsync ( mapper.Map<Permission> ( permissionDto ) );
   if ( !isSuccess )
    return BadRequest ( "updating failed" );
   return Ok ( mapper.Map<PermissionDto> ( updatedPerm ) );
  }
  /// <summary>
  /// Delete permission by name
  /// </summary>
  /// <param name="permissionName"></param>
  /// <returns></returns>
  [HttpDelete ( "DeletePermission" )]
  public async Task<ActionResult> DeletePermissionAsync ( [FromBody] string permissionName )
  {
   if ( string.IsNullOrWhiteSpace ( permissionName ) )
   {
    return BadRequest ( "the missing field is required" );
   }
   int affectedRowsCount = await PermissionService.DeleteAsync ( p => p.PermissionName == permissionName );
   if ( affectedRowsCount == 0 )
   {
    return BadRequest ( "No items deleted" );
   }
   return Ok ( $"{permissionName} has been deleted successfully " );
  }
 }
}
