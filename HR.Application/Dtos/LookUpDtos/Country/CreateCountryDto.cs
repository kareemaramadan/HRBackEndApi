namespace HR.Application.Dtos.LookUpDtos.Country
{
    /// <summary>
    /// Represents a data transfer object (DTO) for creating a new country.
    /// </summary>
    public class CreateCountryDto
    {
        
        public required string ? CountryName_en { get; set; }  
        public required string ? CountryName_ar { get; set; } 

    }
}
