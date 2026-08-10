using System.ComponentModel.DataAnnotations;

namespace HR.Domain.Models.LookUps 
{
    public class Country
    {
        public int Id { get; set; } // Primary key for the Country entity

        [Required]
        public string CountryName_en { get; set; } = string.Empty; // Country name in English
        [Required]
        public string CountryName_ar { get; set; } = string.Empty; // Country name in Arabic

        public virtual ICollection<Governorate>? Governorates { get; set; } // Navigation property for the related governorates


    }
}
