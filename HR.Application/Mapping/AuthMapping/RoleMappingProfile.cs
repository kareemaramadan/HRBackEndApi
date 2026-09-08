using AutoMapper;
using HR.Application.Dtos.RoleDtos;
using HR.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Application.Mapping.AuthMapping
{
    public class RoleMappingProfile:Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<RoleDto, AppRole>().ReverseMap();
            CreateMap<CreateRoleDto,AppRole>();
            CreateMap<UpdateRoleDto, AppRole> ( ).ReverseMap ( );
        }

    }
}
