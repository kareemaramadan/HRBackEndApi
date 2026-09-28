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

 [Route ( "api/[controller]" )]
 [ApiController]
 public class PermissionController ( IBaseService<Permission> PermissionService, IMapper mapper, ILocalizationService localization ) : ControllerBase
 {
  /// <summary>
  /// Get All Permissions in the system.
  /// </summary>
  /// <returns>
  /// 
  /// </returns>
  [HttpGet ( "ReadAllPermissions" )]
  public async Task<ActionResult<ApiResponse<IEnumerable<PermissionDto>>>> GetAllPermissionsAsync ( )
  {
   var (permissions, isSuccess) = await PermissionService.GetAllAsync ( );
   if ( !isSuccess )
    return NotFound ( new ApiResponse<IEnumerable<PermissionDto>>
    {
     Success = false,
     Message = localization.Get ( "itemsNotFound" ),
     Data = null,
     Language = localization.GetLanguage ( )
    } );
   IEnumerable<PermissionDto> getPermissions = mapper.Map<IEnumerable<PermissionDto>> ( permissions );
   ApiResponse<IEnumerable<PermissionDto>> apiResponse = new ApiResponse<IEnumerable<PermissionDto>>
   {
    Success = isSuccess,
    Message = localization.Get ( "itemsRetrieved" ),
    Data = getPermissions.OrderBy ( p => p.PermissionName ).ToList ( ),
    Language = localization.GetLanguage ( )
   };
   return Ok ( apiResponse );
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
  public async Task<ActionResult<ApiResponse<IEnumerable<PermissionDto>>>> GetPermissionIdAsync ( string permissionName )
  {
   if ( string.IsNullOrWhiteSpace ( permissionName ) )
    return BadRequest ( new ApiResponse<IEnumerable<PermissionDto>>
    {
     Success = false,
     Message = localization.Get ( "missingfields" ),
     Data = null,
     Language = localization.GetLanguage ( )
    } );
   var (permission, isSuccess) = await PermissionService.GetByConditionAsync ( p => p.PermissionName == permissionName );
   if ( !isSuccess )
    return NotFound ( new ApiResponse<IEnumerable<PermissionDto>>
    {
     Success = isSuccess,
     Message = localization.Get ( "itemNotFound" ),
     Data = null,
     Language = localization.GetLanguage ( )
    } );

   return Ok ( new ApiResponse<IEnumerable<PermissionDto>>
   {
    Success = isSuccess,
    Message = localization.Get ( "itemRetrieved" ),
    Data = mapper.Map < IEnumerable < PermissionDto >> ( permission),
    Language = localization.GetLanguage ( )
   } );
  }

  /// <summary>
  /// Create a new Permission by Permission Name
  /// </summary>
  /// <param name="createPermission"></param>
  /// <returns>
  /// get all permissions ordered by name
  /// </returns>
  [HttpPost ( "AddNewPermission" )]
  public async Task<ActionResult<ApiResponse<IEnumerable<PermissionDto>>>> AddPermissionAsync ( [FromBody] CreatePermissionDto createPermission )
  {
   if ( !ModelState.IsValid )
   {
    return BadRequest ( new ApiResponse<object>
    {
     Success = false,
     Message = localization.Get ( "validationerror" ),
     Data = ModelState,
     Language = localization.GetLanguage ( )
    } );
   }

   createPermission.PermissionName = char.ToUpper ( createPermission.PermissionName [ 0 ] ) + createPermission.PermissionName [ 1.. ];
   bool isExist = await PermissionService.IsExistAsync ( p => p.PermissionName == createPermission.PermissionName, HttpRequestType.Post );
   if ( isExist )
    return BadRequest ( new ApiResponse<object>
    {
     Success = false,
     Message = localization.Get ( "exists" ),
     Data = null,
     Language = localization.GetLanguage ( )
    } );

   var (permission, isSuccess) = await PermissionService.CreateAsync ( mapper.Map<Permission> ( createPermission ) );
   if ( !isSuccess )
    return BadRequest ( new ApiResponse<ModuleDto>
    {
     Success = isSuccess,
     Message = localization.Get ( "unexpectederror" ),
     Data = null,
     Language = localization.GetLanguage ( )
    } );

   return Created ( "", await GetAllPermissionsAsync ( ) );
  }

  /// <summary>
  /// Update Permission by name and Id
  /// </summary>
  /// <param name="permissionDto"></param>
  /// <returns>
  /// the updated permission
  /// </returns>
  [HttpPut ( "UpdatePermission" )]
  public async Task<ActionResult<ApiResponse<PermissionDto>>> UpdatePermissionAsync ( [FromBody] PermissionDto permissionDto )
  {
   if ( !ModelState.IsValid )
   {
    return BadRequest ( new ApiResponse<object>
    {
     Success = false,
     Message = localization.Get ( "validationerror" ),
     Data = ModelState,
     Language = localization.GetLanguage ( )
    } );
   }
   var (checkPermission, isExist) = await PermissionService.FindAsync ( p => p.PermissionName == permissionDto.PermissionName );
   if ( isExist )
    return BadRequest ( new ApiResponse<object>
    {
     Success = false,
     Message = localization.Get ( "exists" ),
     Data = ModelState,
     Language = localization.GetLanguage ( )
    } );
   var (updatedPerm, isSuccess) = await PermissionService.UpdateAsync ( mapper.Map<Permission> ( permissionDto ) );

   if ( !isSuccess )
    return BadRequest ( new ApiResponse<object>
    {
     Success = false,
     Message = localization.Get ( "updatefailed" ),
     Data = ModelState,
     Language = localization.GetLanguage ( )
    } );
   return Ok (
    new ApiResponse<object>
    {
     Success = false,
     Message = localization.Get ( "updated" ),
     Data = mapper.Map<PermissionDto> ( updatedPerm ),
     Language = localization.GetLanguage ( )
    }
    );
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
    return BadRequest ( new ApiResponse<object>
    {
     Success = false,
     Message = localization.Get ( "missingfields" ),
     Data = null,
     Language = localization.GetLanguage ( )
    } );
   }
   int affectedRowsCount = await PermissionService.DeleteAsync ( p => p.PermissionName == permissionName );
   if ( affectedRowsCount == 0 )
   {
    return BadRequest ( new ApiResponse<object>
    {
     Success = false,
     Message = localization.Get ( "deletionfailed" ),
     Data = null,
     Language = localization.GetLanguage ( )
    } );
   }
   return Ok ( new ApiResponse<object>
   {
    Success = false,
    Message = $"{affectedRowsCount} {localization.Get ( "updated" )}",
    Data = null,
    Language = localization.GetLanguage ( )
   }
    );
  }
 }
}
