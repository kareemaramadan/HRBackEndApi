using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Helpers;
using HR.Application.Interfaces;
using HR.Domain.Models.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRBackEndApi.Controllers
{
    [Route ( "api/[controller]" )]
    [ApiController]
    public class ModulesController ( IBaseService<Module> moduleService, IMapper mapper ) : ControllerBase
    {
        [HttpGet ( "GetAllModules" )]
        public async Task<ActionResult> GetAllModules ( )
        {

            var (Items, IsSuccess) = await moduleService.GetAllAsync ( );
            if ( Items == null )
            {
                return NotFound ( "No modules found." );
            }
            var modules = mapper.Map<IEnumerable<ModuleDto>> ( Items );
            if(modules.Count() > 0)
            return Ok ( modules );

            return BadRequest ( "No Modules found." );
        }

        [HttpGet ( "GetModuleByName" )]
        public async Task<ActionResult> GetModuleByName ( [FromBody] ModuleDto module )
        {
            if ( module.ModuleName_en == null || module.ModuleName_ar == null )
            {
                return BadRequest ( "module name engilsh and arabic are required." );
            }
            var (Items, IsSuccess) = await moduleService.GetByConditionAsync ( m => m.ModuleName_en == module.ModuleName_en && m.ModuleName_ar == module.ModuleName_ar );
            if ( Items == null || !Items.Any ( ) )
            {
                return NotFound ( $"Module with name '{module.ModuleName_en}' and '{module.ModuleName_ar}' not found." );
            }
            return Ok ( mapper.Map<ModuleDto> ( Items.First ( ) ) );

        }

        [HttpPost ( "AddNewModule" )]
        public async Task<ActionResult> AddNewModule ( [FromBody] CreateModuleDto newModule )
        {
            if ( newModule.ModuleName_en == null || newModule.ModuleName_ar == null )
            {
                return BadRequest ( "Module names are required." );
            }
            Module moduleToCreate = mapper.Map<Module> ( newModule );
            var (createdModule, IsSuccess) = await moduleService.CreateAsync ( moduleToCreate, HttpRequestType.Post,
                m => m.ModuleName_en == newModule.ModuleName_en && m.ModuleName_ar == newModule.ModuleName_ar );
            if ( !IsSuccess )
            {
                return BadRequest ( "Failed to create module." );
            }

            return CreatedAtAction ( nameof ( GetModuleByName ), new { moduleName_en = moduleToCreate.ModuleName_en, moduleName_ar = moduleToCreate.ModuleName_ar },mapper.Map<ModuleDto>(createdModule));
        }

        [HttpPut ( "UpdateModule" )]
        public async Task<ActionResult> UpdateModule ( [FromBody] ModuleDto module)
        {
            if ( module.ModuleName_en is null || module.ModuleName_ar is null)
            {
                return BadRequest ( "Module data is required." );
            }
            else
            {
                var (item, IsExist) = await moduleService.FindAsync ( m => m.ModuleId == module.ModuleId );

                if ( !IsExist )
                {
                    return NotFound ( $"this module is not found." );
                }

                if ( module.ModuleImage is null )
                {
                    module.ModuleImage = item.First ( ).ModuleImage;
                }
                

                Module moduleToUpdate = mapper.Map<Module> ( module );

                var (updatedModule, IsSuccess) = await moduleService.UpdateAsync ( moduleToUpdate );

                if ( !IsSuccess )
                {
                    return BadRequest ( "updating failed" );
                }

                return Ok ( mapper.Map<ModuleDto> ( updatedModule ) );
            }


        }

        [HttpDelete ( "DeleteModule" )]
        public async Task<ActionResult> DeleteModule ( [FromBody] ModuleDto module)
        {
            if ( module.ModuleName_en is null || module.ModuleName_ar is null )
            {
                return BadRequest ( "Module Name is required" );
            }
            var (item, IsSuccess) = await moduleService.FindAsync ( m=>m.ModuleName_en == module.ModuleName_en && m.ModuleName_ar == module.ModuleName_ar);
            if ( item == null || !item.Any ( ) )
            {
                return NotFound ( $"This Module is not found." );
            }
            int rowsaffected = await moduleService.DeleteAsync ( m => m.ModuleId == item.First ( ).ModuleId );
            return Ok ( $"The Module is deleted Successfully." );
        }
    }
}
