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
 public class RolePagePermissionsController ( IBaseService<Role_Page_Permission> RolePagePermissionService,IMapper mapper,ILocalizationService localization ) : ControllerBase
 {
  // GET: api/<RolePagePermissionsController>
  [HttpGet ("GetAllPagesPermissionsForRole")]
  public async Task<ActionResult<ApiResponse<IEnumerable<RolePagePermissionsDto>>>> GetAllPagesPermissionsForRole ( [FromBody] GetRolePagePermissionsDto getRolePage )
  {
   ApiResponse<IEnumerable<RolePagePermissionsDto>> response = new ()
   {
    Success=false,
    Data=null,
    Language=localization.GetLanguage ()
   };
   if (!ModelState.IsValid)
   {
    response.Message=localization.Get ("missingfields");
    return response;
   }

   return response;





  }


  // GET api/<RolePagePermissionsController>/5
  [HttpGet ("{id}")]
  public string Get ( int id )
  {
   return "value";
  }

  // POST api/<RolePagePermissionsController>
  [HttpPost]
  public void Post ( [FromBody] string value )
  {
  }

  // PUT api/<RolePagePermissionsController>/5
  [HttpPut ("{id}")]
  public void Put ( int id,[FromBody] string value )
  {
  }

  // DELETE api/<RolePagePermissionsController>/5
  [HttpDelete ("{id}")]
  public void Delete ( int id )
  {
  }
 }
}
