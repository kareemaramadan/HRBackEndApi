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

 [Route ("api/[controller]")]
 [ApiController]
 public class ModulesController ( IBaseService<Module> moduleService,IMapper mapper,ILocalizationService localization ) : ControllerBase
 {

  /// <summary>
  /// Get all Modules data in the system
  /// </summary>
  /// <returns></returns>
  [HttpGet ("GetAllModules")]
  public async Task<ActionResult<ApiResponse<IEnumerable<ModuleDto>>>> GetAllModulesAsync ( )
  {
   ApiResponse<IEnumerable<Module>> items = await moduleService.GetAllAsync ();
   if (!items.Success)
    return NotFound (items);

   IEnumerable<ModuleDto> modules = mapper.Map<IEnumerable<ModuleDto>> (items.Data);
   return Ok (new ApiResponse<IEnumerable<ModuleDto>>
   {
    Success=true,
    Message=items.Message,
    Language=items.Language,
    Data= [.. modules.OrderBy (m => (items.Language is "en" ? m.ModuleName_en : m.ModuleName_ar))]
   });
  }
  /// <summary>
  /// get module data by name
  /// </summary>
  /// <param name="moduleName"></param>
  /// <returns></returns>
  [HttpGet ("GetModuleByName/{moduleName}")]
  public async Task<ActionResult<ApiResponse<ModuleDto>>> GetModuleByNameAsync ( string moduleName )
  {
   if (string.IsNullOrEmpty (moduleName))
    return BadRequest (new ApiResponse<ModuleDto>
    {
     Success=false,
     Message=localization.Get ("missingfields"),
     Data=null,
     Language=localization.GetLanguage ()
    });

   ApiResponse<Module> item = await moduleService.FindItemAsync (m => m.ModuleName_en==moduleName||m.ModuleName_ar==moduleName);
   return (!item.Success) ? NotFound (item) : Ok (new ApiResponse<ModuleDto>
   {
    Success=item.Success,
    Message=item.Message,
    Data= mapper.Map<ModuleDto> (item.Data),
    Language=item.Language
   });
  }
  /// <summary>
  /// Add new Module to the system
  /// </summary>
  /// <param name="newModule"></param>
  /// <returns></returns>
  [HttpPost ("AddNewModule")]
  public async Task<ActionResult<ApiResponse<IEnumerable<ModuleDto>>>> AddNewModuleAsync ( [FromBody] CreateModuleDto newModule )
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
   Module moduleToCreate = mapper.Map<Module> (newModule);
   ApiResponse<IEnumerable<Module>> createdModule = await moduleService.CreateAsyncAndGetAll(moduleToCreate,
       m => (m.ModuleName_en == newModule.ModuleName_en || m.ModuleName_ar == newModule.ModuleName_ar), getAllCriteria: null);

   return (!createdModule.Success) ? BadRequest (createdModule) : Created ("",new ApiResponse<IEnumerable<ModuleDto>>
   {
    Success=createdModule.Success,
    Message=createdModule.Message,
    Data=mapper.Map<IEnumerable<ModuleDto>> (createdModule.Data),
    Language=createdModule.Language
   });
  }
  /// <summary>
  /// Update module image and name
  /// </summary>
  /// <param name="cUModule"></param>
  /// <returns></returns>
  [HttpPut ("UpdateModule")]
  public async Task<ActionResult<ApiResponse<ModuleDto>>> UpdateModuleAsync ( [FromBody] ModuleDto cUModule )
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
   var item = await moduleService.FindItemAsync (m => (m.ModuleName_en==cUModule.ModuleName_en&&m.ModuleName_ar==cUModule.ModuleName_ar)
   &&(m.ModuleId==cUModule.ModuleId));

   if (!item.Success) return NotFound (item);

   cUModule.ModuleImage??=item.Data!.ModuleImage;
   Module moduleToUpdate = mapper.Map<Module> (cUModule);
   var updatedModule = await moduleService.UpdateAsync (moduleToUpdate);
   return (!updatedModule.Success) ? BadRequest (updatedModule) : Ok (new ApiResponse<ModuleDto>
   {
    Success=updatedModule.Success,
    Message=updatedModule.Message,
    Data=mapper.Map<ModuleDto> (updatedModule.Data),
    Language=updatedModule.Language
   });
  }
  /// <summary>
  /// Delete a module from the system
  /// </summary>
  /// <param name="moduleName"></param>
  /// <returns></returns>
  [HttpDelete ("DeleteModule/{moduleName}")]
  public async Task<ActionResult<ApiResponse<int>>> DeleteModuleAsync ( string moduleName )
  {
   if (string.IsNullOrEmpty (moduleName))
    return BadRequest (new ApiResponse<int>
    {
     Success=false,
     Message=localization.Get ("missingfields"),
     Data=0,
     Language=localization.GetLanguage ()
    });
   ApiResponse<Module> deleteditem = await moduleService.FindItemAsync (m => (m.ModuleName_en==moduleName||m.ModuleName_ar==moduleName));
   if (!deleteditem.Success) return BadRequest (deleteditem);
   ApiResponse<int> rowsaffected = await moduleService.DeleteAsync (deleteditem.Data!);
   return (!rowsaffected.Success) ? BadRequest (rowsaffected) : Ok (rowsaffected);
  }
  /// <summary>
  /// delete a group of modules together and module names should be separated by comma
  /// </summary>
  /// <param name="moduleNames"></param>
  /// <returns></returns>
  [HttpDelete ("DeleteBulkOfModules/{moduleNames}")]
  public async Task<ActionResult<ApiResponse<int>>> DeleteBulkModulesAsync ( string moduleNames )
  {
   string [ ] deletedmodules= moduleNames.Split (',');
   int count = deletedmodules.Length;
   int deletedrowsAffected = 0;
   foreach (var moduleName in deletedmodules)
   {
    ApiResponse<Module> deleteditem = await moduleService.FindItemAsync (m => (m.ModuleName_en==moduleName||m.ModuleName_ar==moduleName));
    if (!deleteditem.Success) return BadRequest (deleteditem);
    ApiResponse<int> rowsaffected = await moduleService.DeleteAsync (deleteditem.Data!);
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
