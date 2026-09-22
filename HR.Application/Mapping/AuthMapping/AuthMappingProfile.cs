using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Dtos.RoleDtos;
using HR.Domain.Models.Authorization;
using HR.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Application.Mapping.AuthMapping
{
 public class AuthMappingProfile : Profile
 {
  public AuthMappingProfile ( )
  {
   //AppUser Mapping
   //================
   CreateMap<RegisterDto, AppUser> ( ).ReverseMap ( );
   CreateMap<AppUser, UserRequest> ( ).ReverseMap ( );
   CreateMap<UserProfile, AppUser> ( ).ReverseMap ( );
   //AppRole Mapping
   //================
   CreateMap<RoleDto, AppRole> ( ).ReverseMap ( );
   CreateMap<CreateRoleDto, AppRole> ( );
   CreateMap<UpdateRoleDto, AppRole> ( ).ReverseMap ( );
   //Module Mapping
   //===============
   CreateMap<ModuleDto, Module> ( ).ReverseMap ( );
   //Permission Mapping
   //===================
   CreateMap<PermissionDto, Permission> ( ).ReverseMap ( );
   CreateMap<CreatePermissionDto, Permission> ( ).ReverseMap ( );
  }
 }
}
