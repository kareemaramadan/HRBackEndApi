using AutoMapper;
using HR.Application.Dtos.AppDtos;
using HR.Domain.Models.Authorization;
using HR.Domain.Models.Identity;

namespace HR.Application.Mapping.AuthMapping
{
 public class AuthMappingProfile : Profile
 {
  public AuthMappingProfile ( )
  {
   //AppUser Mapping
   //================

   CreateMap<RegisterDto,AppUser> ().ReverseMap ();
   CreateMap<AppUser,UserRequest> ().ReverseMap ();
   CreateMap<UserProfile,AppUser> ().ReverseMap ();

   //AppRole Mapping
   //================

   CreateMap<RoleDto,AppRole> ().ReverseMap ();
   CreateMap<CreateRoleDto,AppRole> ().ReverseMap ();

   //Module Mapping
   //===============

   CreateMap<ModuleDto,Module> ().ReverseMap ();
   CreateMap<CreateModuleDto,Module> ().ReverseMap ();

   //Permission Mapping
   //===================
   CreateMap<PermissionDto,Permission> ().ReverseMap ();
   CreateMap<CreatePermissionDto,Permission> ().ReverseMap ();

   //ModulePage Mapping
   //==================

   CreateMap<SubModulePagesDto,SubModulePages> ().ReverseMap ();
   CreateMap<SubModulePagesDtowithModule,SubModulePages> ().ReverseMap ();

   //SubModule Mapping
   //==================

   CreateMap<SubModuleDto,SubModules> ().ReverseMap ();
   CreateMap<CreateSubModuleDto,SubModules> ().ReverseMap ();

   //Module_SubModule Mapping
   //========================
   CreateMap<GetModule_SubModulesDto,Module_SubModules> ().ReverseMap ();
   CreateMap<Assign_RemoveModule_SubModuleDto,Module_SubModules> ().ReverseMap ();


   //Role_SubModules Mapping
   //========================
   CreateMap<Role_SubModules,GetRoleSubModules> ().ReverseMap ();
   CreateMap<Assign_Remove_SubModuleToRole,Role_SubModules> ().ReverseMap ();

   //Role_SubModulePages Mapping
   //============================

   CreateMap<Role_SubModulePages,GetRoleSubModulePages> ().ReverseMap ();
   CreateMap<Role_SubModulePages,Assign_Remove_PageToRole> ().ReverseMap ();

  }
 }
}
