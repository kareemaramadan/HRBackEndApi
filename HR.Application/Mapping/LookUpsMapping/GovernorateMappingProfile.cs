using AutoMapper;
using HR.Application.Dtos.LookUpDtos.Governorate;
using HR.Domain.Models.LookUps;


namespace HR.Application.Mapping.LookUpsMapping
{
    public class GovernorateMappingProfile:Profile
    {
        public GovernorateMappingProfile()
        {
            CreateMap<Governorate, GetGovernoratesDto>();

            CreateMap<CreateGovernoratesDto, Governorate>();

            CreateMap<Governorate, UpdateGovernoratesDto>();
            CreateMap<UpdateGovernoratesDto, Governorate>();
        }
    }
}
