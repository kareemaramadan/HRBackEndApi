namespace HR.Application.Dtos.LookUpDtos.Governorate
{
    /// <summary>
    /// Represents a data transfer object (DTO) for creating a new country.
    /// </summary>
    public class CreateGovernoratesDto
    {
        public int Country_Id { get; set; }
        public required string ? GovName_en { get; set; }  
        public required string ? GovName_ar { get; set; } 
        public string GovCode { get; set; } = string.Empty;

    }
}
