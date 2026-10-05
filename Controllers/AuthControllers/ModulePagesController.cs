using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Interfaces;
using HR.Application.Response;
using HR.Domain.Models.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
   var allModulesPages = await ModulePagesService.GetAllAsync (filter: null,include: query => query.Include (mp => mp.Modules!));
   if (!allModulesPages.Success)
   {
    response.Message=localization.Get ("ModulesNotFound");
    return NotFound (response);
   }
   var orderedModulesPages = allModulesPages.Data?.Select (mp => new ModulePagesDtowithModule(mp,currentLanguage)).ToList ().OrderBy(m=>m.ModuleName).ThenBy(m=>m.PageName_en);
   response.Message=localization.Get ("Allmodulepagesretrieved");
   response.Data= mapper.Map<IEnumerable<ModulePagesDtowithModule>>(orderedModulesPages);
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
  [HttpPost("AddPageToModule")]
  public async Task<ActionResult<ApiResponse<IEnumerable<ModulePagesDto>>>> AddPageToModuleAsync([FromBody] CreateModulePagesDto createModulePage)
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
    return BadRequest(response);
   }
   ModulePage modulePage = mapper.Map<ModulePage>(createModulePage);
   ApiResponse<IEnumerable<ModulePage>> addedpage = await ModulePagesService.CreateAsyncAndGetAll(modulePage, mp => (mp.PageName_ar == createModulePage.PageName_ar || mp.PageName_en == createModulePage.PageName_en || mp.PageUrl == createModulePage.PageUrl), mp=>mp.Modules!.ModuleId==createModulePage.ModuleId);
   if (!addedpage.Success) return BadRequest(addedpage);
   return Created("", mapper.Map<IEnumerable<ModulePagesDto>>(addedpage.Data));
  }

  //// PUT api/<ModulePagesController>/5
  //[HttpPut ( "{id}" )]
  //public void Put ( int id, [FromBody] string value )
  //{
  //}

  //// DELETE api/<ModulePagesController>/5
  //[HttpDelete ( "{id}" )]
  //public void Delete ( int id )
  //{
  //}

 }
}
