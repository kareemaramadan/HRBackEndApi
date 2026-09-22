using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Helpers;
using HR.Application.Interfaces;
using HR.Domain.Models.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRBackEndApi.Controllers
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
  [HttpGet ( "getAllPermissions" )]
  public async Task<ActionResult<IEnumerable<PermissionDto>>> GetAllPermissionsAsync ( )
  {
   var (permissions, isSuccess) = await PermissionService.GetAllAsync ( );
   if ( !isSuccess )
    return NotFound ( "No items found." );

   IEnumerable<PermissionDto> getPermissions = mapper.Map<IEnumerable<PermissionDto>> ( permissions );
   return Ok ( getPermissions.OrderBy ( p => p.PermissionName ).ToList ( ) );
  }
  // GET api/<PermissionController>/5
  [HttpGet ( "getPermissionId/{permissionName}" )]
  public async Task<ActionResult<PermissionDto>> GetPermissionIdAsync ( string permissionName )
  {
   if ( string.IsNullOrWhiteSpace ( permissionName ) )
    return BadRequest ( "permission Name is required." );
   var (permission, isSuccess) = await PermissionService.GetByConditionAsync ( p => p.PermissionName == permissionName );
   if ( !isSuccess )
    return NotFound ( $"this permission {permissionName} is not found " );
   return Ok ( permission );
  }

  // POST api/<PermissionController>
  [HttpPost ( "addNewPermission" )]
  public async Task<ActionResult<IEnumerable<PermissionDto>>> PostPermissionAsync ( [FromBody] CreatePermissionDto createPermission )
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

  // PUT api/<PermissionController>/5
  [HttpPut ( "{id}" )]
  public void Put ( int id, [FromBody] string value )
  {
  }

  // DELETE api/<PermissionController>/5
  [HttpDelete ( "{id}" )]
  public void Delete ( int id )
  {
  }
 }
}
