namespace HR.Application.Dtos.LookUpDtos.Country
{
    /// <summary>
    /// Represents a data transfer object (DTO) for reading country information.
    /// </summary>
    public class GetCountryDto
    {
        public int Id { get; set; }
        public string? CountryName_en { get; set; } 
        public string? CountryName_ar { get; set; } 

    }
}
