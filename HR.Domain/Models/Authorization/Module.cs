namespace HR.Domain.Models.Authorization
{
    public class Module
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public byte[] ModuleImage { get; set; } = [];



        public virtual ICollection<ModulePage>? ModulePages { get; set; }


    }
}
