using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Interfaces;
using HR.Domain.Models.Authorization;
using Microsoft.AspNetCore.Mvc;
using HR.Application.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRBackEndApi.Controllers
{
    [Route ( "api/[controller]" )]
    [ApiController]
    public class ModulesController(IBaseService<Module> moduleService, IMapper mapper) : ControllerBase
    {
        [HttpGet("GetAllModules")]
        public async Task<ActionResult> GetAllModules ( )
        {
            
            var (Items, IsSuccess) =  await moduleService.GetAllAsync();
            if( Items == null )
            {
                return NotFound("No modules found.");
            }
            var modules = mapper.Map<IEnumerable<ModuleDto>> ( Items );
            return Ok(modules);
        }

        [HttpGet("GetModuleByName")]
        public async Task<ActionResult> GetModuleByName (string moduleName )
        {
            var (Items, IsSuccess) = await moduleService.GetByConditionAsync ( m => m.ModuleName == moduleName );
            if ( Items == null || !Items.Any ( ) )
            {
                return NotFound ( $"Module with name '{moduleName}' not found." );
            }
            return Ok (mapper.Map<ModuleDto>(Items.First()));  

        }

        [HttpPost("AddNewModule")]
        public async Task<ActionResult> AddNewModule ( [FromBody] ModuleDto newModule )
        {
            if ( newModule == null )
            {
                return BadRequest ( "Module data is required." );
            }
            Module moduleToCreate = mapper.Map<Module>(newModule);
            var (createdModule, IsSuccess) = await moduleService.CreateAsync ( moduleToCreate, HttpRequestType.Post, m => m.ModuleName == newModule.ModuleName );
            if (!IsSuccess)
            {
                return BadRequest ( "Failed to create module." );
            }
            return CreatedAtAction ( nameof ( GetModuleByName ), new { moduleName = moduleToCreate.ModuleName }, moduleToCreate );
        }

        [HttpPut ( "EditModuleByName" )]
        public async Task<ActionResult> EditModuleByName ( [FromBody] ModuleDto module )
        {
            if ( module == null )
            {
                return BadRequest ( "Module data is required." );
            }
            else
            {
                var (item, isexist) = await moduleService.GetByConditionAsync ( m => m.ModuleId == module.ModuleId );

                if ( item == null || !item.Any ( ) )
                {
                    return NotFound ( $"Module with name '{module.ModuleName}' not found." );
                }

                module.ModuleImage = item.First ( ).ModuleImage;

                Module moduleToUpdate = mapper.Map<Module> ( module );

                var (updatedModule, IsSuccess) = await moduleService.UpdateAsync ( moduleToUpdate);

                if ( !IsSuccess )
                {
                    return BadRequest ( "updating failed" );
                }

                return Ok ( mapper.Map<ModuleDto> ( updatedModule ) );
            }


        }

        [HttpDelete ( "DeleteModule" )]
        public async Task<ActionResult> DeleteModule ( string moduleName )
        {
            if ( moduleName == null )
            {
                return BadRequest ( "Module Name is required" );
            }
            var(item,IsSuccess) = await moduleService.FindAsync(m=>m.ModuleName == moduleName);
            if( item == null || !item.Any ( ))
            {
                return NotFound ( $"The Module {moduleName} is not found." );
            }
            int rowsaffected = await moduleService.DeleteAsync ( m => m.ModuleId == item.First ( ).ModuleId );
            return Ok ($"The Module {moduleName} is deleted Successfully.");
        }
    }
}
