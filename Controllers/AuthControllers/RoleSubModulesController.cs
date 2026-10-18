using AutoMapper;
using HR.Application.Dtos.AppDtos;
using HR.Application.Interfaces;
using HR.Application.Response;
using HR.Domain.Models.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRBackEndApi.Controllers.AuthControllers
{
 [ApiController]
 [Route ("api/RoleSubModules")]
 public class RoleSubModulesController ( IBaseService<Role_SubModules> roleSubModulesService,IMapper mapper,ILocalizationService localization,IAuthRelations authRelations ) : ControllerBase
 {
  /// <summary>
  /// Retrieves all submodules assigned to the specified role.
  /// </summary>
  /// <remarks>
  /// Returns localized submodule names for the provided role id. The response Data list will be empty if the role has no submodules.
  /// </remarks>
  /// <example>
  /// GET /api/RoleSubModules/GetAllSubModulesForRole/3
  /// </example>
  /// <param name="roleId">The identifier of the role to query.</param>
  /// <returns>An ApiResponse containing the collection of GetRoleSubModules for the role.</returns>
  [HttpGet (template: "GetAllSubModulesForRole/{RoleId}")]
  public async Task<ActionResult<ApiResponse<IEnumerable<GetRoleSubModules>>>> GetAllSubModulesForRoleAsync ( string roleId )
  {

   ApiResponse<IEnumerable<GetRoleSubModules>> response = new ()
   {
    Success=false,
    Data=null,
    Language=localization.GetLanguage (),
   };

   if (!ModelState.IsValid||string.IsNullOrEmpty (roleId))
   {
    response.Message=localization.Get ("invaliddata");
    return BadRequest (response);
   }
   ApiResponse<IEnumerable<Role_SubModules>> roleSubmodules = await roleSubModulesService.GetAllAsync (
    filter: rms => rms.RoleId==roleId,
    include: query => query.Include (rms => rms.Module).Include (rms => rms.SubModule!).Include (rms => rms.Role!)
    );

   if (!roleSubmodules.Success)
    return BadRequest (roleSubmodules);

   response.Message=roleSubmodules.Message;
   response.Success=roleSubmodules.Success;

   response.Data=roleSubmodules.Data!.Select (msm => new GetRoleSubModules (msm,response.Language)).ToList ();
   return Ok (response);

  }
  /// <summary>
  /// Assigns a submodule to a role. Returns the updated list of role-submodule assignments.
  /// </summary>
  /// <remarks>
  /// If the assignment already exists the service layer may ignore duplicates. Returns the full set of assignments for the role after the operation.
  /// </remarks>
  /// <example>
  /// POST /api/RoleSubModules/AssignSubModuleToRole
  /// { "RoleId": 3, "ModuleId": 1, "SubModuleId": 5 }
  /// </example>
  /// <param name="assign_SubModuleToRole">DTO with RoleId and SubModuleId to assign.</param>
  /// <returns>An ApiResponse with the current GetRoleSubModules collection after assignment.</returns>
  [HttpPost ("AssignSubModuleToRole")]
  public async Task<ActionResult<ApiResponse<IEnumerable<GetRoleSubModules>>>> AssignSubModuleToRole ( [FromBody] Assign_Remove_SubModuleToRole assign_SubModuleToRole )
  {
   ApiResponse<IEnumerable<GetRoleSubModules>> response = new ()
   {
    Success=false,
    Data=null,
    Language=localization.GetLanguage (),
   };
   if (!ModelState.IsValid)
   {
    response.Message=localization.Get ("invaliddata");
    return BadRequest (response);
   }

   Role_SubModules role_SubModules = mapper.Map<Role_SubModules> (assign_SubModuleToRole);
   ApiResponse<IEnumerable<Role_SubModules>> createdItem = await roleSubModulesService.CreateAsyncAndGetAll (
       role_SubModules,
       checkCriteria: rsm => (rsm.RoleId==role_SubModules.RoleId&&rsm.SubModuleId==role_SubModules.SubModuleId),
       getAllCriteria: rsm => (rsm.RoleId==role_SubModules.RoleId&&rsm.ModuleId==role_SubModules.ModuleId),
       include: query => query.Include (rsm => rsm.Role!).Include (rsm => rsm.Module!).Include (msm => msm.SubModule!)
   );

   var authRelationResult = await authRelations.AddSubModuleToRole (role_SubModules.RoleId,role_SubModules.ModuleId,role_SubModules.SubModuleId);

   if (!createdItem.Success) return BadRequest (createdItem);
   if (!authRelationResult.Success) return BadRequest (authRelationResult);

   List<GetRoleSubModules> getfullcreateddata = createdItem.Data!.Select (rsm => new GetRoleSubModules (rsm,createdItem.Language)).ToList ();
   response.Success=true;
   response.Message=createdItem.Message;
   response.Data=getfullcreateddata;
   return Ok (response);
  }
  /// <summary>
  /// Removes a submodule from a role and returns the updated assignments.
  /// </summary>
  /// <remarks>
  /// Deletes the specific role/submodule assignment if present and returns the remaining assignments for the role.
  /// </remarks>
  /// <example>
  /// DELETE /api/RoleSubModules/RemoveSubModuleFromRole
  /// { "RoleId": 3, "ModuleId": 1, "SubModuleId": 5 }
  /// </example>
  /// <param name="remove_SubModuleFromRole">DTO with RoleId and SubModuleId to remove.</param>
  /// <returns>An ApiResponse with the remaining GetRoleSubModules for the role.</returns>
  [HttpDelete ("RemoveSubModuleFromRole")]
  public async Task<ActionResult<ApiResponse<IEnumerable<GetRoleSubModules>>>> RemoveSubModuleFromRole ( [FromBody] Assign_Remove_SubModuleToRole remove_SubModuleFromRole )
  {
   ApiResponse<IEnumerable<GetRoleSubModules>> response = new ()
   {
    Success=false,
    Data=null,
    Language=localization.GetLanguage (),
   };
   if (!ModelState.IsValid)
   {
    response.Message=localization.Get ("invaliddata");
    return BadRequest (response);
   }

   Role_SubModules role_SubModules = mapper.Map<Role_SubModules> (remove_SubModuleFromRole);

   ApiResponse<IEnumerable<Role_SubModules>> deletedItem = await roleSubModulesService.DeleteAsyncAndGetAll (
       filter: rsm => (rsm.RoleId==role_SubModules.RoleId&&rsm.SubModuleId==role_SubModules.SubModuleId),
       GetAllCriteria: rsm => (rsm.RoleId==role_SubModules.RoleId&&rsm.ModuleId==role_SubModules.ModuleId),
       include: query => query.Include (rsm => rsm.Role!).Include (rsm => rsm.Module!).Include (msm => msm.SubModule!)
   );

   var authrelationremovesubmodulefromrole = await authRelations.RemoveSubModuleFromRole (role_SubModules.RoleId,role_SubModules.ModuleId,role_SubModules.SubModuleId);

   if (!deletedItem.Success) return BadRequest (deletedItem);
   if (!authrelationremovesubmodulefromrole.Success) return BadRequest (authrelationremovesubmodulefromrole);

   List<GetRoleSubModules> getfullcreateddata = deletedItem.Data!.Select (rsm => new GetRoleSubModules (rsm,deletedItem.Language)).ToList ();
   response.Success=true;
   response.Message=deletedItem.Message;
   response.Data=getfullcreateddata;
   return Ok (response);
  }
 }
}
