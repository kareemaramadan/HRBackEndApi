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
 public class RolePagesController ( IBaseService<Role_SubModulePages> rolePagesService,IMapper mapper,ILocalizationService localization ) : ControllerBase
 {
  /// <summary>
  /// Retrieves all submodulePages assigned to the specified role.
  /// </summary>
  /// <remarks>
  /// Returns localized submodulePage names for the provided role id. The response Data list will be empty if the role has no submodulePages.
  /// </remarks>
  /// <example>
  /// GET /api/RolePages/GetAllPagesForRole/3
  /// </example>
  /// <param name="roleId">The identifier of the role to query.</param>
  /// <returns>An ApiResponse containing the collection of GetRoleSubModulePages for the role.</returns>
  [HttpGet (template: "GetAllPagesForRole/{RoleId}")]
  public async Task<ActionResult<ApiResponse<IEnumerable<GetRoleSubModulePages>>>> GetAllPagesForRoleAsync ( string roleId )
  {

   ApiResponse<IEnumerable<GetRoleSubModulePages>> response = new ()
   {
    Success=false,
    Data=null,
    Language=localization.GetLanguage (),
   };

   if (!ModelState.IsValid||string.IsNullOrEmpty (roleId))
   {
    response.Message=localization.Get ("invaliddata");
    return response;
   }
   ApiResponse<IEnumerable<Role_SubModulePages>> rolePages = await rolePagesService.GetAllAsync (
    filter: rps => rps.RoleId==roleId,
    include: query => query.Include (rps => rps.Role!).Include (rps => rps.SubModulePage!).ThenInclude (rps => rps.SubModule!)
    );

   if (!rolePages.Success) return BadRequest (rolePages);

   response.Message=rolePages.Message;
   response.Success=rolePages.Success;

   var rolePagesGrouping = rolePages.Data!.Select (rps => new GetRoleSubModulePages (rps,response.Language)).GroupBy (rps => new { rps.RoleId,rps.RoleName,rps.SubModuleName,rps.SubModuleId }).ToList ();
   response.Data=(IEnumerable<GetRoleSubModulePages>?)rolePagesGrouping;
   return Ok (response);

  }
  /// <summary>
  /// Assigns a submodulePage to a role. Returns the updated list of role-submodulePages assignments.
  /// </summary>
  /// <remarks>
  /// If the assignment already exists the service layer may ignore duplicates. Returns the full set of assignments for the role after the operation.
  /// </remarks>
  /// <example>
  /// POST /api/RolePages/AssignPageToRole
  /// { "RoleId": 3, "PageId": 1}
  /// </example>
  /// <param name="assign_PageToRole">DTO with RoleId and PageId to assign.</param>
  /// <returns>An ApiResponse with the current GetRoleSubModulePages collection after assignment.</returns>
  [HttpPost ("AssignPageToRole")]
  public async Task<ActionResult<ApiResponse<IEnumerable<GetRoleSubModulePages>>>> AssignPageToRole ( [FromBody] Assign_Remove_PageToRole assign_PageToRole )
  {
   ApiResponse<IEnumerable<GetRoleSubModulePages>> response = new ()
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
   Role_SubModulePages rolePages = mapper.Map<Role_SubModulePages> (assign_PageToRole);
   ApiResponse<IEnumerable<Role_SubModulePages>> createdItem = await rolePagesService.CreateAsyncAndGetAll (rolePages,
      checkCriteria: rsp => (rsp.RoleId==rolePages.RoleId&&rsp.PageId==rolePages.PageId),
      getAllCriteria: (rsp => (rsp.RoleId==rolePages.RoleId)),
      include: (query => query.Include (rsp => rsp.Role!).Include (rsp => rsp.SubModulePage!).ThenInclude (rsp => rsp.SubModule!))
      );

   if (!createdItem.Success) { return BadRequest (createdItem); }

   var rolePagesGrouping = createdItem.Data!.Select (rps => new GetRoleSubModulePages (rps,response.Language)).GroupBy (rps => new { rps.RoleId,rps.RoleName,rps.SubModuleName,rps.SubModuleId }).ToList ();

   response.Success=true;
   response.Message=createdItem.Message;
   response.Data=(IEnumerable<GetRoleSubModulePages>?)rolePagesGrouping;
   return Ok (response);
  }
  /// <summary>
  /// Removes a submodulePage from a role and returns the updated assignments.
  /// </summary>
  /// <remarks>
  /// Deletes the specific role/submodulePage assignment if present and returns the remaining assignments for the role.
  /// </remarks>
  /// <example>
  /// DELETE /api/RolePages/RemovePageFromRole
  /// { "RoleId": 3, "PageId": 1}
  /// </example>
  /// <param name="remove_PageToRole">DTO with RoleId and PageId to remove.</param>
  /// <returns>An ApiResponse with the remaining GetRoleSubModulePages for the role.</returns>
  [HttpDelete ("RemoveSubModuleFromRole")]
  public async Task<ActionResult<ApiResponse<IEnumerable<GetRoleSubModulePages>>>> RemoveSubModuleFromRole ( [FromBody] Assign_Remove_PageToRole remove_PageToRole )
  {
   ApiResponse<IEnumerable<GetRoleSubModulePages>> response = new ()
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
   Role_SubModulePages rolePages = mapper.Map<Role_SubModulePages> (remove_PageToRole);

   ApiResponse<IEnumerable<Role_SubModulePages>> deletedItem = await rolePagesService.DeleteAsyncAndGetAll (
      filter: rsp => (rsp.RoleId==rolePages.RoleId&&rsp.PageId==rolePages.PageId),
      GetAllCriteria: (rsp => (rsp.RoleId==rolePages.RoleId)),
      include: (query => query.Include (rsp => rsp.Role!).Include (rsp => rsp.SubModulePage!).ThenInclude (rsp => rsp.SubModule!))
      );

   if (!deletedItem.Success) { return BadRequest (deletedItem); }

   var rolePagesGrouping = deletedItem.Data!.Select (rps => new GetRoleSubModulePages (rps,response.Language)).GroupBy (rps => new { rps.RoleId,rps.RoleName,rps.SubModuleName,rps.SubModuleId }).ToList ();

   response.Success=true;
   response.Message=deletedItem.Message;
   response.Data=(IEnumerable<GetRoleSubModulePages>?)rolePagesGrouping;
   return Ok (response);
  }
 }
}
