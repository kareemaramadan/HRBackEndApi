using AutoMapper;
using HR.Application.Dtos.LookUpDtos.Grades;
using HR.Domain.Models.LookUps;

namespace HR.Application.Mapping.LookUpsMapping
{
    public class GradeMappingProfile:Profile
    {
        public GradeMappingProfile()
        {
            CreateMap<Grade, GetGradesDto>();

            CreateMap<CreateGradeDto, Grade>();

            CreateMap<Grade, UpdateGradeDto>();
            CreateMap<UpdateGradeDto, Grade>();
        }
    }
}
