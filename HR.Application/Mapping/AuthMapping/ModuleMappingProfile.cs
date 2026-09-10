using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Domain.Models.Authorization;
using HR.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Application.Mapping.AuthMapping
{
    public class ModuleMappingProfile : Profile
    {
        public ModuleMappingProfile ( )
        {
            CreateMap<ModuleDto, Module> ( ).ReverseMap ( );
        }

    }
}
