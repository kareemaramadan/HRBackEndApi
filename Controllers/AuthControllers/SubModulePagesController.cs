using AutoMapper;
using HR.Application.Dtos.AppDtos;
using HR.Application.Interfaces;
using HR.Application.Response;
using HR.Domain.Models.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRBackEndApi.Controllers.AuthControllers
{
 [Route ("api/[controller]")]
 [ApiController]
 public class SubModulePagesController ( IBaseService<SubModulePages> ModulePagesService,IMapper mapper,ILocalizationService localization ) : ControllerBase
 {
  /// <summary>
  /// Retrieves all module pages, including their associated module information, ordered by module name and page name.
  /// </summary>
  /// <remarks>
  /// Returns all module pages with localized submodule names. If no pages exist returns an empty list.
  /// </remarks>
  /// <example>
  /// GET /api/SubModulePages/GetALLModulesPages
  /// // Success (200)
  /// { "success": true, "data": [ { "SubModuleId": 5, "SubModuleName": "Payroll", "PageId": 1, "PageName_en": "Overview" } ], "message": "Allmodulepagesretrieved" }
  /// </example>
  /// <returns>ApiResponse with SubModulePagesDtowithModule collection.</returns>
  [HttpGet ("GetALLModulesPages")]
  public async Task<ActionResult<ApiResponse<IEnumerable<SubModulePagesDtowithModule>>>> ReadAllPagesAsync ( )
  {
   string currentLanguage = localization.GetLanguage ();
   ApiResponse<IEnumerable<SubModulePagesDtowithModule>> response = new ()
   {
    Success=false,
    Language=currentLanguage,
    Data=null
   };
   var allModulesPages = await ModulePagesService.GetAllAsync (filter: null,include: query => query.Include (mp => mp.SubModule!));
   if (!allModulesPages.Success)
   {
    response.Message=localization.Get ("ModulesNotFound");
    return NotFound (response);
   }
   var orderedModulesPages = allModulesPages.Data?
       .Select (mp => new SubModulePagesDtowithModule (mp,currentLanguage))
       .OrderBy (m => m.SubModuleName)
       .ThenBy (m => m.PageName_en)
       .ToList ();
   response.Message=localization.Get ("Allmodulepagesretrieved");
   response.Data=orderedModulesPages;
   response.Success=true;
   return Ok (response);
  }
  /// <summary>
  /// Retrieves module pages by module ID, including their associated module information, ordered by page name.
  /// </summary>
  /// <remarks>
  /// Returns pages for the provided module id. If no pages are found returns NotFound (404).
  /// </remarks>
  /// <example>
  /// GET /api/SubModulePages/GetModulePagesByModuleId/5
  /// // Success (200)
  /// { "success": true, "data": [ { "PageId": 1, "PageName_en": "Overview" } ], "message": "modulepagesretrieved" }
  /// // NotFound (404)
  /// { "success": false, "message": "NoModulePagesfound'5'" }
  /// </example>
  /// <returns>ApiResponse with SubModulePagesDto collection.</returns>
  [HttpGet ("GetModulePagesByModuleId/{moduleId}")]
  public async Task<ActionResult<ApiResponse<IEnumerable<SubModulePagesDto>>>> ReadModulePagesByModuleIdAsync ( int moduleId )
  {
   string currentLanguage = localization.GetLanguage ();
   ApiResponse<IEnumerable<SubModulePagesDto>> response = new ()
   {
    Success=false,
    Language=currentLanguage,
    Data=null
   };
   ApiResponse<IEnumerable<SubModulePages>> modulepages = await ModulePagesService.GetAllAsync ((mp => mp.SubModule!=null&&(mp.SubModule.SubModuleId==moduleId)));
   if (!modulepages.Success)
   {
    response.Message=$"{localization.Get ("NoModulePagesfound")}'{moduleId}'";
    return NotFound (response);
   }
   response.Message=localization.Get ("modulepagesretrieved");
   response.Data=mapper.Map<IEnumerable<SubModulePagesDto>> (modulepages.Data);
   response.Success=true;
   return Ok (response);
  }
  /// <summary>
  /// Adds a new page to a module, ensuring that the page name and URL are unique within the module, and returns the all pages information for the specified module ID.
  /// </summary>
  /// <remarks>
  /// Creates a new page and returns Created (201) with all pages for the module. Validation errors return BadRequest (400).
  /// </remarks>
  /// <example>
  /// POST /api/SubModulePages/AddPageToModule
  /// { "SubModuleId": 5, "PageName_en": "Overview", "PageUrl": "/payroll/overview" }
  /// // Success (201)
  /// [ { "PageId": 10, "PageName_en": "Overview", "PageUrl": "/payroll/overview" } ]
  /// </example>
  /// <returns>ApiResponse with created and all pages for the module id.</returns>
  [HttpPost ("AddPageToModule")]
  public async Task<ActionResult<ApiResponse<IEnumerable<SubModulePagesDto>>>> AddPageToModuleAsync ( [FromBody] SubModulePagesDto createModulePage )
  {
   ApiResponse<IEnumerable<SubModulePagesDto>> response = new ()
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
   SubModulePages modulePage = mapper.Map<SubModulePages> (createModulePage);
   ApiResponse<IEnumerable<SubModulePages>> addedpage = await ModulePagesService.CreateAsyncAndGetAll (modulePage,mp => (mp.PageName_ar==createModulePage.PageName_ar||mp.PageName_en==createModulePage.PageName_en||mp.PageUrl==createModulePage.PageUrl),mp => mp.SubModule!.SubModuleId==createModulePage.SubModuleId);
   if (!addedpage.Success)
    return BadRequest (addedpage);
   return Created ("",mapper.Map<IEnumerable<SubModulePagesDto>> (addedpage.Data));
  }
  /// <summary> 
  /// Updates an existing module page and returns the updated list of module pages.
  /// </summary>
  /// <remarks>
  /// Updates the page metadata and returns the updated list. Validation errors will return BadRequest.
  /// </remarks>
  /// <example>
  /// PUT /api/SubModulePages/UpdateModulePage
  /// { "SubModuleId": 5, "PageId": 10, "PageName_en": "Overview Updated", "PageUrl": "/payroll/overview" }
  /// </example>
  /// <returns>ApiResponse with updated SubModulePagesDto collection.</returns>
  [HttpPut ("UpdateModulePage")]
  public async Task<ActionResult<ApiResponse<IEnumerable<SubModulePagesDto>>>> UpdateModulePageAsync ( [FromBody] SubModulePagesDto updateModulePage )
  {
   ApiResponse<IEnumerable<SubModulePagesDto>> response = new ()
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
   SubModulePages modulePage = mapper.Map<SubModulePages> (updateModulePage);

   ApiResponse<IEnumerable<SubModulePages>> updatedPage = await ModulePagesService.UpdateAsyncAndGetAll (modulePage,mp => (mp.PageName_ar==updateModulePage.PageName_ar||mp.PageName_en==updateModulePage.PageName_en||mp.PageUrl==updateModulePage.PageUrl),mp => mp.SubModule!.SubModuleId==modulePage.SubModuleId);
   return (updatedPage.Success) ? Ok (mapper.Map<IEnumerable<SubModulePagesDto>> (updatedPage.Data)) : BadRequest (updatedPage);
  }
  /// <summary>
  /// Deletes a module page by its name and module ID, returning the result of the deletion operation, along with the updated list of module pages for the specified module ID.
  /// </summary>
  /// <param name="pageName"></param>
  /// <param name="moduleId"></param>
  [HttpDelete ("DeleteModulePage/{pageName}/{moduleId}")]
  public async Task<ActionResult<ApiResponse<IEnumerable<SubModulePagesDto>>>> DeleteModulePageAsync ( string pageName,int moduleId )
  {
   ApiResponse<IEnumerable<SubModulePagesDto>> response = new ()
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
   ApiResponse<IEnumerable<SubModulePages>> deletedPage = await ModulePagesService.DeleteAsyncAndGetAll (mp => ((mp.PageName_en==pageName||mp.PageName_ar==pageName)&&mp.SubModuleId==moduleId),mp => mp.SubModule!.SubModuleId==moduleId);
   if (!deletedPage.Success) return BadRequest (deletedPage);

   response.Data=mapper.Map<IEnumerable<SubModulePagesDto>> (deletedPage.Data);
   response.Message=deletedPage.Message;
   response.Success=deletedPage.Success;
   return Ok (response);
  }
 }
}
