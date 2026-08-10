using AutoMapper;
using HR.Application.Dtos.LookUpDtos.Country;
using HR.Domain.Models.LookUps;



namespace HR.Application.Mapping.LookUpsMapping
{
    public class CountryMappingProfile:Profile
    {
        public CountryMappingProfile()
        {
            CreateMap<Country, GetCountryDto>();

            CreateMap<CreateCountryDto, Country>();

            CreateMap <Country, UpdateCountryDto>();
            CreateMap<UpdateCountryDto, Country>();
        }
    }
}
