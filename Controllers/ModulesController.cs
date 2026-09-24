using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Helpers;
using HR.Application.Interfaces;
using HR.Domain.Models.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRBackEndApi.Controllers
{

 [Route ( "api/[controller]" )]
 [ApiController]
 public class ModulesController ( IBaseService<Module> moduleService, IMapper mapper ) : ControllerBase
 {
  /// <summary>
  /// Get all Modules data in the system
  /// </summary>
  /// <returns></returns>
  [HttpGet ( "getAllModules" )]
  public async Task<ActionResult<IEnumerable<ModuleDto>>> getAllModulesAsync ( )
  {
   var (items, isSuccess) = await moduleService.GetAllAsync ( );
   if ( items == null )
    return NotFound ( "No modules found." );

   var modules = mapper.Map<IEnumerable<ModuleDto>> ( items );
   return Ok ( modules.OrderBy ( m => m.ModuleName_en ).ToList ( ) );
  }
  /// <summary>
  /// get module data by name
  /// </summary>
  /// <param name="moduleName"></param>
  /// <returns></returns>
  [HttpGet ( "getModuleByName/{moduleName}" )]
  public async Task<ActionResult<ModuleDto>> getModuleByNameAsync ( string moduleName )
  {
   if ( moduleName is null )
    return BadRequest ( "module name is required." );

   var (items, isSuccess) = await moduleService.FindAsync ( m => m.ModuleName_en == moduleName || m.ModuleName_ar == moduleName );
   if ( items == null || !items.Any ( ) )
    return NotFound ( $"No module is found by this name {moduleName}" );

   return Ok ( mapper.Map<ModuleDto> ( items.First ( ) ) );
  }
  /// <summary>
  /// Add new Module to the system
  /// </summary>
  /// <param name="newModule"></param>
  /// <returns></returns>
  [HttpPost ( "addNewModule" )]
  public async Task<ActionResult<ModuleDto>> addNewModuleAsync ( [FromBody] ModuleDto newModule )
  {
   if ( newModule.ModuleName_en == null || newModule.ModuleName_ar == null )
    return BadRequest ( "Module names are required." );

   Module moduleToCreate = mapper.Map<Module> ( newModule );
   var (createdModule, IsSuccess) = await moduleService.CreateAsync ( moduleToCreate, HttpRequestType.Post,
       m => m.ModuleName_en == newModule.ModuleName_en && m.ModuleName_ar == newModule.ModuleName_ar );
   if ( !IsSuccess )
    return BadRequest ( "Failed to create module." );

   return CreatedAtAction ( nameof ( getModuleByNameAsync ), new { moduleName = newModule.ModuleName_en }, mapper.Map<ModuleDto> ( createdModule ) );
  }
  /// <summary>
  /// Update module image and name
  /// </summary>
  /// <param name="cUModule"></param>
  /// <returns></returns>
  [HttpPut ( "updateModule" )]
  public async Task<ActionResult<ModuleDto>> updateModuleAsync ( [FromBody] ModuleDto cUModule )
  {
   if ( cUModule.ModuleName_en is null || cUModule.ModuleName_ar is null )
    return BadRequest ( "Module data is required." );

    var (item, isExist) = await moduleService.FindAsync ( m => ( m.ModuleName_en == cUModule.ModuleName_en && m.ModuleName_ar == cUModule.ModuleName_ar ) 
    && m.ModuleId == cUModule.ModuleId );

    if ( !isExist )
     return NotFound ( $"this module is not found." );

    if ( cUModule.ModuleImage is null )
     cUModule.ModuleImage = item?.First ( ).ModuleImage;
    
    Module moduleToUpdate = mapper.Map<Module> ( cUModule );
    var (updatedModule, isSuccess) = await moduleService.UpdateAsync ( moduleToUpdate );
    if ( !isSuccess )
     return BadRequest ( "updating failed" );

    return Ok ( mapper.Map<ModuleDto> ( updatedModule ) );
  }
  /// <summary>
  /// check module function used in delete methods
  /// </summary>
  /// <param name="moduleName"></param>
  /// <returns></returns>
  private async Task<bool> checkModules ( string moduleName )
  {
   if ( moduleName is null )
   {
    return false;
   }
   var (item, IsSuccess) = await moduleService.FindAsync ( m => m.ModuleName_en == moduleName || m.ModuleName_ar == moduleName );
   if ( item == null || !item.Any ( ) )
   {
    return false;
   }
   int rowsaffected = await moduleService.DeleteAsync ( m => m.ModuleId == item.First ( ).ModuleId );

   return ( rowsaffected == 1 ) ? true : false;
  }
  /// <summary>
  /// Delete a module from the system
  /// </summary>
  /// <param name="moduleName"></param>
  /// <returns></returns>
  [HttpDelete ( "deleteModule/{moduleName}" )]
  public async Task<ActionResult<string>> deleteModuleAsync ( string moduleName )
  {
   bool itemIsDeleted = await checkModules ( moduleName );


   return ( itemIsDeleted ) ? Ok ( $"The Module is deleted Successfully." ) : BadRequest ( "Something done during deletion" );
  }
  /// <summary>
  /// delete a group of modules together
  /// </summary>
  /// <param name="moduleNames"></param>
  /// <returns></returns>
  [HttpDelete ( "deleteBulkOfModules/{moduleNames}" )]
  public async Task<ActionResult<string>> deleteBulkModulesAsync ( string [ ] moduleNames )
  {
   int count = moduleNames.Count ( );
   int rowsAffected = 0;
   foreach ( var moduleName in moduleNames )
   {
    bool isDeleted = await checkModules ( moduleName );
    if ( isDeleted ) { rowsAffected++; }
   }
   if ( rowsAffected == 0 ) {
    return BadRequest ( "No items deleted" );
   }
   return Ok ( $"{rowsAffected} items are deleted successfully from {count} items are selected" );
  }

 }
}
