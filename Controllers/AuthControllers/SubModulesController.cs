using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Interfaces;
using HR.Application.Response;
using HR.Domain.Models.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRBackEndApi.Controllers.AuthControllers
{
 [Route ("api/[controller]")]
 [ApiController]
 public class SubModulesController ( IBaseService<SubModules> subModuleService,IMapper mapper,ILocalizationService localization ) : ControllerBase
 {
  /// <summary>
  /// Get all SubModules data in the system
  /// </summary>
  /// <returns></returns>
  [HttpGet ("GetAllSubModules")]
  public async Task<ActionResult<ApiResponse<IEnumerable<SubModuleDto>>>> GetAllSubModulesAsync ( )
  {
   ApiResponse<IEnumerable<SubModules>> items = await subModuleService.GetAllAsync ();
   if (!items.Success)
    return NotFound (items);

   IEnumerable<SubModuleDto> subModules = mapper.Map<IEnumerable<SubModuleDto>> (items.Data);
   return Ok (new ApiResponse<IEnumerable<SubModuleDto>>
   {
    Success=true,
    Message=items.Message,
    Language=items.Language,
    Data= [.. subModules.OrderBy (m => (items.Language is "en" ? m.SubModuleName_en : m.SubModuleName_ar))]
   });
  }
  /// <summary>
  /// get sub module data by name
  /// </summary>
  /// <param name="subModuleName"></param>
  /// <returns></returns>
  [HttpGet ("GetSubModuleByName/{subModuleName}")]
  public async Task<ActionResult<ApiResponse<SubModuleDto>>> GetSubModuleByNameAsync ( string subModuleName )
  {
   if (string.IsNullOrEmpty (subModuleName))
    return BadRequest (new ApiResponse<SubModuleDto>
    {
     Success=false,
     Message=localization.Get ("missingfields"),
     Data=null,
     Language=localization.GetLanguage ()
    });

   ApiResponse<SubModules> item = await subModuleService.FindItemAsync (m => m.SubModuleName_en==subModuleName||m.SubModuleName_ar==subModuleName);
   return (!item.Success) ? NotFound (item) : Ok (new ApiResponse<SubModuleDto>
   {
    Success=item.Success,
    Message=item.Message,
    Data=mapper.Map<SubModuleDto> (item.Data),
    Language=item.Language
   });
  }
  /// <summary>
  /// Add new SubModule to the system
  /// </summary>
  /// <param name="newSubModule"></param>
  /// <returns></returns>
  [HttpPost ("AddNewSubModule")]
  public async Task<ActionResult<ApiResponse<IEnumerable<SubModuleDto>>>> AddNewSubModuleAsync ( [FromBody] CreateSubModuleDto newSubModule )
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
   SubModules CreateSubModule = mapper.Map<SubModules> (newSubModule);
   ApiResponse<IEnumerable<SubModules>> createdSubModule = await subModuleService.CreateAsyncAndGetAll (CreateSubModule,
       m => (m.SubModuleName_en==newSubModule.SubModuleName_en||m.SubModuleName_ar==newSubModule.SubModuleName_ar),getAllCriteria: null);

   return (!createdSubModule.Success) ? BadRequest (createdSubModule) : Created ("",new ApiResponse<IEnumerable<SubModuleDto>>
   {
    Success=createdSubModule.Success,
    Message=createdSubModule.Message,
    Data=mapper.Map<IEnumerable<SubModuleDto>> (createdSubModule.Data),
    Language=createdSubModule.Language
   });
  }
  /// <summary>
  /// Update sub module name
  /// </summary>
  /// <param name="cUSubModule"></param>
  /// <returns></returns>
  [HttpPut ("UpdateSubModule")]
  public async Task<ActionResult<ApiResponse<SubModuleDto>>> UpdateSubModuleAsync ( [FromBody] SubModuleDto cUSubModule )
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
   var item = await subModuleService.FindItemAsync (m => (m.SubModuleName_en==cUSubModule.SubModuleName_en&&m.SubModuleName_ar==cUSubModule.SubModuleName_ar)
   &&(m.SubModuleId==cUSubModule.SubModuleId));

   if (!item.Success) return NotFound (item);

   SubModules moduleToUpdate = mapper.Map<SubModules> (cUSubModule);
   var updatedModule = await subModuleService.UpdateAsync (moduleToUpdate);
   return (!updatedModule.Success) ? BadRequest (updatedModule) : Ok (new ApiResponse<SubModuleDto>
   {
    Success=updatedModule.Success,
    Message=updatedModule.Message,
    Data=mapper.Map<SubModuleDto> (updatedModule.Data),
    Language=updatedModule.Language
   });
  }
  /// <summary>
  /// Delete a sub module from the system
  /// </summary>
  /// <param name="subModuleName"></param>
  /// <returns></returns>
  [HttpDelete ("DeleteSubModule/{subModuleName}")]
  public async Task<ActionResult<ApiResponse<int>>> DeleteSubModuleAsync ( string subModuleName )
  {
   if (string.IsNullOrEmpty (subModuleName))
    return BadRequest (new ApiResponse<int>
    {
     Success=false,
     Message=localization.Get ("missingfields"),
     Data=0,
     Language=localization.GetLanguage ()
    });
   ApiResponse<SubModules> deleteditem = await subModuleService.FindItemAsync (m => (m.SubModuleName_en==subModuleName||m.SubModuleName_ar==subModuleName));
   if (!deleteditem.Success) return BadRequest (deleteditem);
   ApiResponse<int> rowsaffected = await subModuleService.DeleteAsync (deleteditem.Data!);
   return (!rowsaffected.Success) ? BadRequest (rowsaffected) : Ok (rowsaffected);
  }
  /// <summary>
  /// delete a group of sub modules together and sub module names should be separated by comma
  /// </summary>
  /// <param name="subModuleNames"></param>
  /// <returns></returns>
  [HttpDelete ("DeleteBulkOfSubModules/{subModuleNames}")]
  public async Task<ActionResult<ApiResponse<int>>> DeleteBulkModulesAsync ( string subModuleNames )
  {
   string [ ] deletedmodules = subModuleNames.Split (',');
   int count = deletedmodules.Length;
   int deletedrowsAffected = 0;
   foreach (var subModuleName in deletedmodules)
   {
    ApiResponse<SubModules> deleteditem = await subModuleService.FindItemAsync (m => (m.SubModuleName_en==subModuleName||m.SubModuleName_ar==subModuleName));
    if (!deleteditem.Success) return BadRequest (deleteditem);
    ApiResponse<int> rowsaffected = await subModuleService.DeleteAsync (deleteditem.Data!);
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
