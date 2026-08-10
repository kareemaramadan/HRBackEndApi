namespace HR.Application.Dtos.LookUpDtos.Grades
{
    /// <summary>
    /// Represents a data transfer object (DTO) for creating a new grade.
    /// </summary>
    public class CreateGradeDto
    {
        public required string GradeName_en { get; set; }
        public required string GradeName_ar { get; set; }
        public required int priority { get; set; }
        public required int percentage { get; set; }

    }
}
