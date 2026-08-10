namespace HR.Application.Dtos.LookUpDtos.Grades
{
    /// <summary>
    /// Represents a data transfer object (DTO) for reading country information.
    /// </summary>
    public class GetGradesDto
    {
        public int Id { get; set; }
        public string? GradeName_en { get; set; }
        public string? GradeName_ar { get; set; }
        public int? priority { get; set; }
        public int? percentage { get; set; }

    }
}
