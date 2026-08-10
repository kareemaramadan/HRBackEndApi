using AutoMapper;
using HR.Application.Dtos.LookUpDtos.Company;
using HR.Domain.Models.LookUps;

namespace HR.Application.Mapping.LookUpsMapping
{
    public class CompanyMappingProfile:Profile
    {
        public CompanyMappingProfile()
        {
            CreateMap<Company, GetCompanyDto>();

            CreateMap<CreateCompanyDto, Company>();

            CreateMap<Company, UpdateCompanyDto>();
            CreateMap<UpdateCompanyDto, Company>();
        }
    }
}
