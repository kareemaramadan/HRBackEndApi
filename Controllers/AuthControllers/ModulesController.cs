using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Helpers;
using HR.Application.Interfaces;
using HR.Application.Response;
using HR.Domain.Models.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRBackEndApi.Controllers.AuthControllers
{

 [Route ( "api/[controller]" )]
 [ApiController]
 public class ModulesController ( IBaseService<Module> moduleService, IMapper mapper, ILocalizationService localization ) : ControllerBase
 {
  /// <summary>
  /// Get all Modules data in the system
  /// </summary>
  /// <returns></returns>
  [HttpGet ( "getAllModules" )]
  public async Task<ActionResult<ApiResponse<IEnumerable<ModuleDto>>>> getAllModulesAsync ( )
  {
   var (items, isSuccess) = await moduleService.GetAllAsync ( );
   if ( items.Count() == 0 )
    return NotFound ( new ApiResponse<IEnumerable<ModuleDto>>
    {
     Success = false,
     Message = localization.Get ( "itemsNotFound" ),
     Data = null,
     Language = localization.GetLanguage ( )
    } );

   var modules = mapper.Map<IEnumerable<ModuleDto>> ( items );
   return Ok ( new ApiResponse<IEnumerable<ModuleDto>>
   {
    Success = true,
    Message = localization.Get ( "itemsRetrieved" ),
    Data = modules.OrderBy ( m => m.ModuleName_en ).ToList ( ),
    Language = localization.GetLanguage ( )
   }
 );
  }

  /// <summary>
  /// get module data by name
  /// </summary>
  /// <param name="moduleName"></param>
  /// <returns></returns>
  [HttpGet ( "getModuleByName/{moduleName}" )]
  public async Task<ActionResult<ApiResponse<ModuleDto>>> getModuleByNameAsync ( string moduleName )
  {
   if ( moduleName is null )
    return BadRequest ( new ApiResponse<ModuleDto>
    {
     Success = false,
     Message = localization.Get ( "missingfields" ),
     Data = null,
     Language = localization.GetLanguage ( )
    } );

   var (items, isSuccess) = await moduleService.FindAsync ( m => m.ModuleName_en == moduleName || m.ModuleName_ar == moduleName );
   if ( items == null || !items.Any ( ) )
    return NotFound ( new ApiResponse<ModuleDto>
    {
     Success = isSuccess,
     Message = localization.Get ( "itemNotFound" ),
     Data = null,
     Language = localization.GetLanguage ( )
    } );

   return Ok ( new ApiResponse<ModuleDto>
   {
    Success = isSuccess,
    Message = localization.Get ( "itemRetrieved" ),
    Data = mapper.Map<ModuleDto> ( items.First ( ) ),
    Language = localization.GetLanguage ( )
   } );
  }

  /// <summary>
  /// Add new Module to the system
  /// </summary>
  /// <param name="newModule"></param>
  /// <returns></returns>
  [HttpPost ( "addNewModule" )]
  public async Task<ActionResult<ApiResponse<ModuleDto>>> addNewModuleAsync ( [FromBody] createModuleDto newModule )
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
   Module moduleToCreate = mapper.Map<Module> ( newModule );
   var (createdModule, IsSuccess) = await moduleService.CreateAsync ( moduleToCreate, HttpRequestType.Post,
       m => m.ModuleName_en == newModule.ModuleName_en && m.ModuleName_ar == newModule.ModuleName_ar );
   if ( !IsSuccess )
    return BadRequest ( new ApiResponse<ModuleDto>
    {
     Success = IsSuccess,
     Message = localization.Get ( "unexpectederror" ),
     Data = null,
     Language = localization.GetLanguage ( )
    } );

   return Created ( "", await getAllModulesAsync() );
  }

  /// <summary>
  /// Update module image and name
  /// </summary>
  /// <param name="cUModule"></param>
  /// <returns></returns>
  [HttpPut ( "updateModule" )]
  public async Task<ActionResult<ApiResponse<ModuleDto>>> updateModuleAsync ( [FromBody] ModuleDto cUModule )
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
   var (item, isExist) = await moduleService.FindAsync ( m => ( m.ModuleName_en == cUModule.ModuleName_en && m.ModuleName_ar == cUModule.ModuleName_ar )
   && m.ModuleId == cUModule.ModuleId );

   if (isExist )
    return BadRequest ( new ApiResponse<object>
    {
     Success = false,
     Message = localization.Get ( "exists" ),
     Data = ModelState,
     Language = localization.GetLanguage ( )
    } );

   if ( cUModule.ModuleImage is null )
    cUModule.ModuleImage = item?.First ( ).ModuleImage;

   Module moduleToUpdate = mapper.Map<Module> ( cUModule );
   var (updatedModule, isSuccess) = await moduleService.UpdateAsync ( moduleToUpdate );
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
     Data = mapper.Map<ModuleDto> ( updatedModule ),
     Language = localization.GetLanguage ( )
    }
    );
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


   return ( itemIsDeleted ) ? Ok ( new ApiResponse<object>
   {
    Success = itemIsDeleted,
    Message = localization.Get ( "deleted" ),
    Data = null,
    Language = localization.GetLanguage ( )
   } ) : BadRequest ( new ApiResponse<object>
   {
    Success = itemIsDeleted,
    Message = localization.Get ( "deletionfailed" ),
    Data = null,
    Language = localization.GetLanguage ( )
   } );
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
   if ( rowsAffected == 0 )
   {
    return BadRequest ( new ApiResponse<object>
    {
     Success = false,
     Message = localization.Get ( "deletionfailed" ),
     Data = null,
     Language = localization.GetLanguage ( )
    } );
   }
   return Ok (new ApiResponse<object>
     {
      Success = false,
      Message = $"{rowsAffected} {localization.Get ( "updated" )} " ,
      Data = null,
      Language = localization.GetLanguage ( )
     }
    );
  }

 }
}
