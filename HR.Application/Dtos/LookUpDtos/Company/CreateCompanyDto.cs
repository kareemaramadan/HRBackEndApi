namespace HR.Application.Dtos.LookUpDtos.Company
{
    /// <summary>
    /// Represents a data transfer object (DTO) for creating a new country.
    /// </summary>
    public class CreateCompanyDto
    {
      public int Country_Id {  get; set; }
      public int Gov_Id {  get; set; }
      public int City_Id {  get; set; }
      public string? CompName_en {  get; set; }
      public string? CompName_ar {get; set;}
      public string? Address_en {get; set;}
      public string? Address_ar {get; set;}
      public byte[]? CompLogo { get; set; }
    }
}
