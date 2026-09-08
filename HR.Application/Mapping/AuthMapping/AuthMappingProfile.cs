using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Application.Mapping.AuthMapping
{
    public class AuthMappingProfile:Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<RegisterDto, AppUser>().ReverseMap();
            CreateMap<AppUser, UserRequest>().ReverseMap(); 
            CreateMap<UserProfile, AppUser> ( ).ReverseMap ( );
        }
    }
}
