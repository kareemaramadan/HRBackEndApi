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
  /// Retrieves all Submodule pages, including their associated Submodule information, ordered by Submodule name and page name.
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
  [HttpGet ("GetALLSubModulePages")]
  public async Task<ActionResult<ApiResponse<IEnumerable<SubModulePagesDtowithModule>>>> GetAllSubModulePagesAsync ( )
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
  /// Retrieves Submodule pages by Submodule ID, including their associated Submodule information, ordered by page name.
  /// </summary>
  /// <remarks>
  /// Returns pages for the provided Submodule id. If no pages are found returns NotFound (404).
  /// </remarks>
  /// <example>
  /// GET /api/SubModulePages/GetModulePagesByModuleId/5
  /// // Success (200)
  /// { "success": true, "data": [ { "PageId": 1, "PageName_en": "Overview" } ], "message": "modulepagesretrieved" }
  /// // NotFound (404)
  /// { "success": false, "message": "NoModulePagesfound'5'" }
  /// </example>
  /// <returns>ApiResponse with SubModulePagesDto collection.</returns>
  [HttpGet ("GetSubModulePagesBySubModuleId/{submoduleId}")]
  public async Task<ActionResult<ApiResponse<IEnumerable<SubModulePagesDto>>>> ReadSubModulePagesBySubModuleIdAsync ( int submoduleId )
  {
   string currentLanguage = localization.GetLanguage ();
   ApiResponse<IEnumerable<SubModulePagesDto>> response = new ()
   {
    Success=false,
    Language=currentLanguage,
    Data=null
   };
   ApiResponse<IEnumerable<SubModulePages>> submodulepages = await ModulePagesService.GetAllAsync ((mp => mp.SubModule!=null&&(mp.SubModule.SubModuleId==submoduleId)));
   if (!submodulepages.Success)
   {
    response.Message=$"{localization.Get ("NoPagesfound")}'{submoduleId}'";
    return NotFound (response);
   }
   response.Message=localization.Get ("submodulepagesretrieved");
   response.Data=mapper.Map<IEnumerable<SubModulePagesDto>> (submodulepages.Data);
   response.Success=true;
   return Ok (response);
  }
  /// <summary>
  /// Adds a new page to a Submodule, ensuring that the page name and URL are unique within the Submodule, and returns the all pages information for the specified Submodule ID.
  /// </summary>
  /// <remarks>
  /// Creates a new page and returns Created (201) with all pages for the Submodule. Validation errors return BadRequest (400).
  /// </remarks>
  /// <example>
  /// POST /api/SubModulePages/AddPageToSubModule
  /// { "SubModuleId": 5, "PageName_en": "Overview", "PageUrl": "/payroll/overview" }
  /// // Success (201)
  /// [ { "PageId": 10, "PageName_en": "Overview", "PageUrl": "/payroll/overview" } ]
  /// </example>
  /// <returns>ApiResponse with created and all pages for the Submodule id.</returns>
  [HttpPost ("AddPageToSubModule")]
  public async Task<ActionResult<ApiResponse<IEnumerable<SubModulePagesDto>>>> AddPageToSubModuleAsync ( [FromBody] SubModulePagesDto createSubModulePage )
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
   SubModulePages modulePage = mapper.Map<SubModulePages> (createSubModulePage);
   ApiResponse<IEnumerable<SubModulePages>> addedpage = await ModulePagesService.CreateAsyncAndGetAll (modulePage,mp => (mp.PageName_ar==createSubModulePage.PageName_ar||mp.PageName_en==createSubModulePage.PageName_en||mp.PageUrl==createSubModulePage.PageUrl),mp => mp.SubModule!.SubModuleId==createSubModulePage.SubModuleId);
   if (!addedpage.Success)
    return BadRequest (addedpage);
   return Created ("",mapper.Map<IEnumerable<SubModulePagesDto>> (addedpage.Data));
  }
  /// <summary> 
  /// Updates an existing Submodule page and returns the updated list of Submodule pages.
  /// </summary>
  /// <remarks>
  /// Updates the page metadata and returns the updated list. Validation errors will return BadRequest.
  /// </remarks>
  /// <example>
  /// PUT /api/SubModulePages/UpdateSubModulePage
  /// { "SubModuleId": 5, "PageId": 10, "PageName_en": "Overview Updated", "PageUrl": "/payroll/overview" }
  /// </example>
  /// <returns>ApiResponse with updated SubModulePagesDto collection.</returns>
  [HttpPut ("UpdateSubModulePage")]
  public async Task<ActionResult<ApiResponse<IEnumerable<SubModulePagesDto>>>> UpdateModulePageAsync ( [FromBody] SubModulePagesDto updateSubModulePage )
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
   SubModulePages modulePage = mapper.Map<SubModulePages> (updateSubModulePage);

   ApiResponse<IEnumerable<SubModulePages>> updatedPage = await ModulePagesService.UpdateAsyncAndGetAll (modulePage,mp => (mp.PageName_ar==updateSubModulePage.PageName_ar||mp.PageName_en==updateSubModulePage.PageName_en||mp.PageUrl==updateSubModulePage.PageUrl),mp => mp.SubModule!.SubModuleId==modulePage.SubModuleId);
   return (updatedPage.Success) ? Ok (mapper.Map<IEnumerable<SubModulePagesDto>> (updatedPage.Data)) : BadRequest (updatedPage);
  }
  /// <summary>
  /// Deletes a Submodule page by its name and Submodule ID, returning the result of the deletion operation, along with the updated list of Submodule pages for the specified Submodule ID.
  /// </summary>
  /// <param name="pageName"></param>
  /// <param name="submoduleId"></param>
  [HttpDelete ("DeleteSubModulePage/{pageName}/{submoduleId}")]
  public async Task<ActionResult<ApiResponse<IEnumerable<SubModulePagesDto>>>> DeleteModulePageAsync ( string pageName,int submoduleId )
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
   ApiResponse<IEnumerable<SubModulePages>> deletedPage = await ModulePagesService.DeleteAsyncAndGetAll (mp => ((mp.PageName_en==pageName||mp.PageName_ar==pageName)&&mp.SubModuleId==submoduleId),mp => mp.SubModule!.SubModuleId==submoduleId);
   if (!deletedPage.Success) return BadRequest (deletedPage);

   response.Data=mapper.Map<IEnumerable<SubModulePagesDto>> (deletedPage.Data);
   response.Message=deletedPage.Message;
   response.Success=deletedPage.Success;
   return Ok (response);
  }
 }
}
