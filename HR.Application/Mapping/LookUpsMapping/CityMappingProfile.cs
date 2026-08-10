using AutoMapper;
using HR.Application.Dtos.LookUpDtos.City;
using HR.Domain.Models.LookUps;



namespace HR.Application.Mapping.LookUpsMapping
{
    public class CityMappingProfile:Profile
    {
        public CityMappingProfile()
        {
            CreateMap<City, GetCityDto>();

            CreateMap<CreateCityDto, City>();

            CreateMap<City, UpdateCityDto>();
            CreateMap<UpdateCityDto, City>();
        }
    }
}
