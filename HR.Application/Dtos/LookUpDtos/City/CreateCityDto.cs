namespace HR.Application.Dtos.LookUpDtos.City
{
    /// <summary>
    /// Represents a data transfer object (DTO) for creating a new country.
    /// </summary>
    public class CreateCityDto
    {
        public int Country_Id { get; set; }
        public int Gov_Id { get; set; }
        public required string ? CityName_en { get; set; }  
        public required string ? CityName_ar { get; set; } 

    }   
}
