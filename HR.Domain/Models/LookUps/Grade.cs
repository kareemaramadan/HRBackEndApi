namespace HR.Domain.Models.LookUps 
{
    public class Grade
    {
        public int Id { get; set; }
        public required string GradeName_en { get; set; }
        public required string GradeName_ar { get; set; }
        public required int priority { get; set; }
        public required int percentage { get; set; }
    }
}
