using AutoMapper;
using HR.Application.Dtos.AppDtos;
using HR.Application.Interfaces;
using HR.Application.Response;
using HR.Domain.Models.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRBackEndApi.Controllers.AuthControllers
{
 [ApiController]
 [Route ("api/[controller]")]
 public class ModuleSubModulesController ( IBaseService<Module_SubModules> module_SubModulesService,IMapper mapper,ILocalizationService localization ) : ControllerBase
 {
  /// <summary>
  /// Retrieves the submodules assigned to a specific module.
  /// </summary>
  /// <remarks>
  /// This endpoint returns localized submodule information for the provided module id.
  /// Use the localization service to change returned language.
  /// </remarks>
  /// <example>
  /// GET /api/ModuleSubModules/GetSubModulesForModule/5
  /// </example>
  /// <param name="moduleId">The id of the module to fetch submodules for.</param>
  /// <returns>An ApiResponse containing a collection of GetModule_SubModulesDto for the given module.</returns>
  [HttpGet ("GetSubModulesForModule/{moduleId}")]
  public async Task<ActionResult<ApiResponse<IEnumerable<GetModule_SubModulesDto>>>> GetSubModulesForModuleAsync ( int moduleId )
  {

   ApiResponse<IEnumerable<GetModule_SubModulesDto>> response = new ()
   {
    Success=false,
    Data=null,
    Language=localization.GetLanguage (),
   };

   if (!ModelState.IsValid)
   {
    response.Message=localization.Get ("invaliddata");
    return response;
   }
   ApiResponse<IEnumerable<Module_SubModules>> submodules = await module_SubModulesService.GetAllAsync (
    filter: msm => msm.ModuleId==moduleId,
    include: query => query.Include (ms => ms.Module).Include (ms => ms.SubModule!)
    );
   if (!submodules.Success) return BadRequest (submodules);

   response.Message=submodules.Message;
   response.Success=submodules.Success;

   response.Data = submodules.Data!.Select(msm => new GetModule_SubModulesDto(msm, response.Language)).ToList();
   return Ok (response);

  }

  /// <summary>
  /// Assigns a submodule to a module. If the assignment already exists the service will handle deduplication.
  /// Returns the updated list of submodule assignments for the module.
  /// </summary>
  /// <remarks>
  /// The request body must include ModuleId and SubModuleId. The service will return the full list for that module after assignment.
  /// </remarks>
  /// <example>
  /// POST /api/ModuleSubModules/AssignSubModuleToModule
  /// { "ModuleId": 5, "SubModuleId": 12 }
  /// </example>
  /// <param name="assign_module_SubModule">DTO containing the ModuleId and SubModuleId to assign.</param>
  /// <returns>An ApiResponse with the current collection of GetModule_SubModulesDto after the assignment.</returns>
  [HttpPost ("AssignSubModuleToModule")]
  public async Task<ActionResult<ApiResponse<IEnumerable<GetModule_SubModulesDto>>>> AssignSubModuleToModule ( [FromBody] Assign_RemoveModule_SubModuleDto assign_module_SubModule )
  {
   ApiResponse<IEnumerable<GetModule_SubModulesDto>> response = new ()
   {
    Success=false,
    Data=null,
    Language=localization.GetLanguage (),
   };
   if (!ModelState.IsValid)
   {
    response.Message=localization.Get ("invaliddata");
    return response;
   }
   Module_SubModules module_SubModules = mapper.Map<Module_SubModules> (assign_module_SubModule);
   ApiResponse<IEnumerable<Module_SubModules>> createdItem = await module_SubModulesService.CreateAsyncAndGetAll (module_SubModules,
      checkCriteria: msm => (msm.ModuleId==module_SubModules.ModuleId&&msm.SubModuleId==module_SubModules.SubModuleId),
      getAllCriteria: (msm => (msm.ModuleId==module_SubModules.ModuleId)),
      include: (query => query.Include (msm => msm.Module!).Include (msm => msm.SubModule!))
      );

   if (!createdItem.Success) { return BadRequest (createdItem); }
   List<GetModule_SubModulesDto> getfullcreateddata = createdItem.Data!.Select(msm => new GetModule_SubModulesDto(msm, createdItem.Language)).ToList();
   response.Success=true;
   response.Message=createdItem.Message;
   response.Data=getfullcreateddata;
   return Ok (response);
  }

  /// <summary>
  /// Removes a submodule from the specified module and returns the updated assignments.
  /// </summary>
  /// <remarks>
  /// Provide ModuleId and SubModuleId to remove the association. The response contains remaining assignments.
  /// </remarks>
  /// <example>
  /// DELETE /api/ModuleSubModules/RemoveSubModuleFromModule
  /// { "ModuleId": 5, "SubModuleId": 12 }
  /// </example>
  /// <param name="Remove_module_SubModule">DTO containing the ModuleId and SubModuleId to remove.</param>
  /// <returns>An ApiResponse with the remaining GetModule_SubModulesDto for the module.</returns>
  [HttpDelete ("RemoveSubModuleFromModule")]
  public async Task<ActionResult<ApiResponse<IEnumerable<GetModule_SubModulesDto>>>> RemoveSubModuleFromModuleAsync ( [FromBody] Assign_RemoveModule_SubModuleDto Remove_module_SubModule )
  {
   ApiResponse<IEnumerable<GetModule_SubModulesDto>> response = new ()
   {
    Success=false,
    Data=null,
    Language=localization.GetLanguage (),
   };

   if (!ModelState.IsValid)
   {
    response.Message=localization.Get ("invaliddata");
    return response;
   }

   Module_SubModules module_SubModules = mapper.Map<Module_SubModules> (Remove_module_SubModule);
   ApiResponse<IEnumerable<Module_SubModules>> deleteditem = await module_SubModulesService.DeleteAsyncAndGetAll (
       filter: msm => (msm.ModuleId==module_SubModules.ModuleId&&msm.SubModuleId==module_SubModules.SubModuleId),
       GetAllCriteria: (msm => (msm.ModuleId==module_SubModules.ModuleId)),
       include: (query => query.Include (msm => msm.Module!).Include (msm => msm.SubModule!))
       );

   if (!deleteditem.Success) { return BadRequest (deleteditem); }
   List<GetModule_SubModulesDto> getitems = deleteditem.Data!.Select(msm => new GetModule_SubModulesDto(msm, deleteditem.Language)).ToList();
   response.Success=deleteditem.Success;
   response.Data=getitems;
   response.Message=deleteditem.Message;
   return Ok (response);
  }

 }
}