using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Interfaces;
using HR.Application.Response;
using HR.Domain.Models.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRBackEndApi.Controllers.AuthControllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class Module_SubModulesController(IBaseService<Module_SubModules> module_SubModulesService, IMapper mapper, ILocalizationService localization) : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        [HttpGet("GetSubModulesForModule/{moduleId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Module_SubModulesDto>>>> GetSubModulesForModuleAsync(int moduleId)
        {

            ApiResponse<IEnumerable<Module_SubModulesDto>> response = new()
            {
                Success = false,
                Data = null,
                Language = localization.GetLanguage(),
            };

            if (!ModelState.IsValid)
            {
                response.Message = localization.Get("invaliddata");
                return response;
            }
            ApiResponse<IEnumerable<Module_SubModules>> submodules = await module_SubModulesService.GetAllAsync(
             filter: msm => msm.ModuleId == moduleId,
             include: query => query.Include(ms => ms.Module).Include(ms => ms.SubModule!)
             );
            if (!submodules.Success) return BadRequest(submodules);

            response.Message = submodules.Message;
            response.Success = submodules.Success;

            response.Data = mapper.Map<IEnumerable<Module_SubModulesDto>>(submodules.Data!.Select(msm => new Module_SubModulesDto(msm, response.Language)).ToList());
            return Ok(response);

        }

        [HttpPost("AddSubModuleToModule")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Module_SubModulesDto>>>> AddSubModuleToModule([FromBody] Module_SubModuleDto module_SubModule)
        {
            ApiResponse<IEnumerable<Module_SubModulesDto>> response = new()
            {
                Success = false,
                Data = null,
                Language = localization.GetLanguage(),
            };
            if (!ModelState.IsValid)
            {
                response.Message = localization.Get("invaliddata");
                return response;
            }
            Module_SubModules module_SubModules = mapper.Map<Module_SubModules>(module_SubModule);
            ApiResponse<IEnumerable<Module_SubModules>> createdItem = await module_SubModulesService.CreateAsyncAndGetAll(module_SubModules,
               checkCriteria: msm => (msm.ModuleId == module_SubModule.ModuleId && msm.SubModuleId == module_SubModule.SubModuleId),
               getAllCriteria: (msm => (msm.ModuleId == module_SubModule.ModuleId)),
               include: (query => query.Include(msm => msm.Module!).Include(msm => msm.SubModule!))
               );

            if (!createdItem.Success) { return BadRequest(createdItem); }
            var getfullcreateddata = createdItem.Data!.Select(msm => new Module_SubModulesDto(msm, createdItem.Language));
            response.Success = true;
            response.Message = createdItem.Message;
            response.Data = [.. mapper.Map<IEnumerable<Module_SubModulesDto>>(getfullcreateddata)];
            return Ok(response);
        }

        [HttpDelete("DeleteSubModuleFromModule")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Module_SubModulesDto>>>> DeleteSubModuleAsync([FromBody] Module_SubModuleDto module_SubModule)
        {
            ApiResponse<IEnumerable<Module_SubModulesDto>> response = new()
            {
                Success = false,
                Data = null,
                Language = localization.GetLanguage(),
            };

            if (!ModelState.IsValid)
            {
                response.Message = localization.Get("invaliddata");
                return response;
            }
            ApiResponse<IEnumerable<Module_SubModules>> deleteditem = await module_SubModulesService.DeleteAsyncAndGetAll(
                filter: msm => (msm.ModuleId == module_SubModule.ModuleId && msm.SubModuleId == module_SubModule.SubModuleId),
                GetAllCriteria: (msm => (msm.ModuleId == module_SubModule.ModuleId)),
                include: (query => query.Include(msm => msm.Module!).Include(msm => msm.SubModule!))
                );

            if (!deleteditem.Success) { return BadRequest(deleteditem); }
            var getitems = deleteditem.Data!.Select(msm => new Module_SubModulesDto(msm, deleteditem.Language)).ToList();
            response.Success = deleteditem.Success;
            response.Data = mapper.Map<IEnumerable<Module_SubModulesDto>>(getitems);
            response.Message = deleteditem.Message;
            return Ok(response);
        }

    }
}