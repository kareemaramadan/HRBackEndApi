using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Interfaces;
using HR.Application.Response;
using HR.Domain.Models.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRBackEndApi.Controllers.AuthControllers
{
 [Route ("api/[controller]")]
 [ApiController]
 public class ModulePagesController ( IBaseService<ModulePage> ModulePagesService,IMapper mapper,ILocalizationService localization ) : ControllerBase
 {
  /// <summary>
  /// Retrieves all module pages, including their associated module information, ordered by module name and page name.
  /// </summary>
  /// <returns>
  /// A list of all module pages.
  /// </returns>
  [HttpGet ("GetALLModulesPages")]
  public async Task<ActionResult<ApiResponse<IEnumerable<ModulePagesDtowithModule>>>> ReadAllPagesAsync ( )
  {
   string currentLanguage = localization.GetLanguage ();
   ApiResponse<IEnumerable<ModulePagesDtowithModule>> response = new ()
   {
    Success=false,
    Language=currentLanguage,
    Data=null
   };
   var allModulesPages = await ModulePagesService.GetAllAsync (filter:null,include: query => query.Include (mp => mp.Modules!));
   if (!allModulesPages.Success)
   {
    response.Message=localization.Get ("ModulesNotFound");
    return NotFound (response);
   }
   var orderedModulesPages = allModulesPages.Data?.Select (mp => new ModulePagesDtowithModule (mp,currentLanguage)).ToList ().OrderBy (m => m.ModuleName).ThenBy (m => m.PageName_en);
   response.Message=localization.Get ("Allmodulepagesretrieved");
   response.Data=mapper.Map<IEnumerable<ModulePagesDtowithModule>> (orderedModulesPages);
   response.Success=true;
   return Ok (response);
  }
  /// <summary>
  /// Retrieves module pages by module ID, including their associated module information, ordered by page name.
  /// </summary>
  /// <returns>
  /// A list of module pages for the specified module ID.
  /// </returns>
  [HttpGet ("GetModulePagesByModuleId/{moduleId}")]
  public async Task<ActionResult<ApiResponse<IEnumerable<ModulePagesDto>>>> ReadModulePagesByModuleIdAsync ( int moduleId )
  {
   string currentLanguage = localization.GetLanguage ();
   ApiResponse<IEnumerable<ModulePagesDto>> response = new ()
   {
    Success=false,
    Language=currentLanguage,
    Data=null
   };
   ApiResponse<IEnumerable<ModulePage>> modulepages = await ModulePagesService.GetAllAsync ((mp => mp.Modules!=null&&(mp.Modules.ModuleId==moduleId)));
   if (!modulepages.Success)
   {
    response.Message=$"{localization.Get ("NoModulePagesfound")}'{moduleId}'";
    return NotFound (response);
   }
   response.Message=localization.Get ("modulepagesretrieved");
   response.Data=mapper.Map<IEnumerable<ModulePagesDto>> (modulepages.Data);
   response.Success=true;
   return Ok (response);
  }
  /// <summary>
  /// Adds a new page to a module, ensuring that the page name and URL are unique within the module, and returns the all pages information for the specified module ID.
  /// </summary>
  /// <returns>
  /// A list of the createda and all module pages for the specified module ID.
  /// </returns>
  [HttpPost ("AddPageToModule")]
  public async Task<ActionResult<ApiResponse<IEnumerable<ModulePagesDto>>>> AddPageToModuleAsync ( [FromBody] ModulePagesDto createModulePage )
  {
   ApiResponse<IEnumerable<ModulePagesDto>> response = new ()
   {
    Success=false,
    Language=localization.GetLanguage (),
    Data=null
   };
   if (!ModelState.IsValid)
   {
    response.Message=localization.Get ("missingfields");
    return BadRequest (response);
   }
   ModulePage modulePage = mapper.Map<ModulePage> (createModulePage);
   ApiResponse<IEnumerable<ModulePage>> addedpage = await ModulePagesService.CreateAsyncAndGetAll (modulePage,mp => (mp.PageName_ar==createModulePage.PageName_ar||mp.PageName_en==createModulePage.PageName_en||mp.PageUrl==createModulePage.PageUrl),mp => mp.Modules!.ModuleId==createModulePage.ModuleId);
   if (!addedpage.Success)
    return BadRequest (addedpage);
   return Created ("",mapper.Map<IEnumerable<ModulePagesDto>> (addedpage.Data));
  }
  /// <summary> 
  /// Updates an existing module page and returns the updated list of module pages.
  /// </summary>
  /// <returns>
  /// A list of the updated module pages.
  /// </returns>
  [HttpPut ("UpdateModulePage")]
  public async Task<ActionResult<ApiResponse<IEnumerable<ModulePagesDto>>>> UpdateModulePageAsync ( [FromBody] ModulePagesDto updateModulePage )
  {
   ApiResponse<IEnumerable<ModulePagesDto>> response = new ()
   {
    Success=false,
    Language=localization.GetLanguage (),
    Data=null,
    Message=""
   };
   if (!ModelState.IsValid)
   {
    response.Message=localization.Get ("missingfields");
    return BadRequest (response);
   }
   ModulePage modulePage = mapper.Map<ModulePage> (updateModulePage);

   ApiResponse<IEnumerable<ModulePage>> updatedPage = await ModulePagesService.UpdateAsyncAndGetAll (modulePage,mp => (mp.PageName_ar==updateModulePage.PageName_ar||mp.PageName_en==updateModulePage.PageName_en||mp.PageUrl==updateModulePage.PageUrl),mp => mp.Modules!.ModuleId==modulePage.ModuleId);
   return (updatedPage.Success) ? Ok (mapper.Map<IEnumerable<ModulePagesDto>> (updatedPage.Data)) : BadRequest (updatedPage);
  }
  /// <summary>
  /// Deletes a module page by its name and module ID, returning the result of the deletion operation, along with the updated list of module pages for the specified module ID.
  /// </summary>
  /// <param name="pageName"></param>
  /// <param name="moduleId"></param>
  [HttpDelete ("DeleteModulePage/{pageName}/{moduleId}")]
  public async Task<ActionResult<ApiResponse<IEnumerable<ModulePagesDto>>>> DeleteModulePageAsync ( string pageName,int moduleId )
  {
   ApiResponse<IEnumerable<ModulePagesDto>> response = new ()
   {
    Success=false,
    Language=localization.GetLanguage (),
    Data=null,
    Message=""
   }; 
   if (string.IsNullOrEmpty (pageName))
   {
    response.Message=localization.Get ("missingfields");
    return BadRequest (response);
   }
   ApiResponse<IEnumerable<ModulePage>> deletedPage = await ModulePagesService.DeleteAsyncAndGetAll (mp => ((mp.PageName_en==pageName||mp.PageName_ar==pageName)&&mp.ModuleId==moduleId),mp => mp.Modules!.ModuleId==moduleId);
   if (!deletedPage.Success) return BadRequest (deletedPage);

   response.Data=mapper.Map<IEnumerable<ModulePagesDto>> (deletedPage.Data);
   response.Message=deletedPage.Message;
   response.Success=deletedPage.Success;
   return Ok (response);
  }
 }
}
