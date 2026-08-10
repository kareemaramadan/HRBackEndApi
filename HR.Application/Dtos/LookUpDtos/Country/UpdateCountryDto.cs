namespace HR.Application.Dtos.LookUpDtos.Country
{
    /// <summary>
    /// Represents a data transfer object (DTO) for updating country information.
    /// </summary>
    public class UpdateCountryDto
    {
        public required string? CountryName_en { get; set; }
        public required string? CountryName_ar { get; set; }
    }
}
