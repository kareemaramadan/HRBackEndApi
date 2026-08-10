namespace HR.Application.Dtos.LookUpDtos.Governorate
{
    /// <summary>
    /// Represents a data transfer object (DTO) for reading country information.
    /// </summary>
    public class GetGovernoratesDto
    {
        public int Id { get; set; }
        public int Country_Id { get; set; }
        public required string? GovName_en { get; set; }
        public required string? GovName_ar { get; set; }
        public string GovCode { get; set; } = string.Empty;

    }
}
