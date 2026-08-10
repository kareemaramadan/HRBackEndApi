namespace HR.Application.Dtos.LookUpDtos.City
{
    /// <summary>
    /// Represents a data transfer object (DTO) for reading country information.
    /// </summary>
    public class GetCityDto
    {
        public int Id { get; set; }
        public int Country_Id { get; set; }
        public int Gov_Id { get; set; }
        public string? CityName_en { get; set; } 
        public string? CityName_ar { get; set; } 

    }
}
