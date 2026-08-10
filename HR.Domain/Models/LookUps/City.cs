namespace HR.Domain.Models.LookUps
{
    public class City
    {
        public int Id { get; set; } // Primary key for the City entity
        public int Country_Id { get; set; } // Foreign key referencing the Country entity
        public int Gov_Id { get; set; } // Foreign key referencing the Governorate entity
        public string CityName_en { get; set; } = string.Empty; // City name in English
        public string CityName_ar { get; set; } = string.Empty; // City name in Arabic


        public virtual Country? Country { get; set; } // Navigation property for the related Country entity
        public virtual Governorate? Governorate { get; set; } // Navigation property for the related Governorate entity
        public virtual ICollection<Company>? Companies { get; set; } // Navigation property for the related companies
    }
}
