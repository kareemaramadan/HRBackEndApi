using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Interfaces;
using HR.Domain.Models.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRBackEndApi.Controllers.AuthControllers
{
 [Route ( "api/[controller]" )]
 [ApiController]
 public class ModulePagesController ( IBaseService<ModulePage> ModulePagesService, IMapper mapper ) : ControllerBase
 {

  [HttpGet ( "GetALLModulesPages" )]
  public async Task<ActionResult<IEnumerable<ModulePagesDto>>> ReadAllPagesAsync ( )
  {
   var (allModulesPages, isSuccess) = await ModulePagesService.GetAllAsync ( );
   if ( !isSuccess )
    return NoContent ( );

   IEnumerable<ModulePage> orderPages = allModulesPages
    .OrderBy ( mp => mp.Modules != null ? mp.Modules.ModuleName_en : string.Empty )
    .ThenBy ( mp => mp.PageName_en );
   return Ok ( mapper.Map<ModulePagesDto> ( orderPages ) );

  }

  // GET api/<ModulePagesController>/5
  [HttpGet ( "GetModulePagesByModule" )]
  public async Task<ActionResult<IEnumerable<ModulePagesDto>>> ReadModulePagesAsync (string ModuleName)
  {

   var (modulePages, isSuccess) = await ModulePagesService.GetByConditionAsync ( mp => 
   mp.Modules != null && 
   ( mp.Modules.ModuleName_en == ModuleName || mp.Modules.ModuleName_ar == ModuleName ) );
   if ( !isSuccess || modulePages is null || !modulePages.Any ( ) )
    return NotFound ( $"No module pages found matching: '{ModuleName}'" );

   return Ok ( mapper.Map<ModulePagesDto>(modulePages));
  }

  // POST api/<ModulePagesController>
  [HttpPost("AddPageToModule")]
  public async Task<ActionResult> AddPageToModuleAsync ( [FromBody] CreateModulePagesDto createModulePage )
  {
   if ( createModulePage is null )
    return BadRequest ( "fill the missing fields" );
   var (modulePageChecked, isExists) = await ModulePagesService.FindAsync ( mp => ( mp.PageName_ar == createModulePage.PageName_ar || mp.PageName_en == createModulePage.PageName_en || mp.PageUrl == createModulePage.PageUrl ) );
   if ( isExists )
    return BadRequest ( "this name or URL already exists before" );

   ModulePage modulePage = mapper.Map<ModulePage> ( createModulePage );

   var ( addedpage,isSuccess ) = await ModulePagesService.CreateAsync ( modulePage);
   if ( addedpage is null && !isSuccess )
    return BadRequest ("Error during Creation");

   return Created ( "", mapper.Map<ModulePagesDto> ( addedpage ) );
  }

  // PUT api/<ModulePagesController>/5
  [HttpPut ( "{id}" )]
  public void Put ( int id, [FromBody] string value )
  {
  }

  // DELETE api/<ModulePagesController>/5
  [HttpDelete ( "{id}" )]
  public void Delete ( int id )
  {
  }
 }
}
