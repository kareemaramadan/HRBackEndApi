namespace HR.Application.Dtos.LookUpDtos.City
{
    /// <summary>
    /// Represents a data transfer object (DTO) for updating country information.
    /// </summary>
    public class UpdateCityDto
    {
        public int Id { get; set; }
        public required string? CityName_en { get; set; }
        public required string? CityName_ar { get; set; }
    }
}
