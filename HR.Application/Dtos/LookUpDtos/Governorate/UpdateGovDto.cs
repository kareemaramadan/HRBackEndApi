namespace HR.Application.Dtos.LookUpDtos.Governorate
{
    /// <summary>
    /// Represents a data transfer object (DTO) for updating country information.
    /// </summary>
    public class UpdateGovernoratesDto
    {
        public int Id { get; set; }
        public required string? GovName_en { get; set; }
        public required string? GovName_ar { get; set; }
        public string GovCode { get; set; } = string.Empty;
    }
}
