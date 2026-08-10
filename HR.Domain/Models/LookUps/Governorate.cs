namespace HR.Domain.Models.LookUps
{
    public class Governorate
    {
        public int Id { get; set; } // Primary key for the Governorate entity
        public string GovName_en { get; set; } = string.Empty; // Governorate name in English
        public string GovName_ar { get; set; } = string.Empty; // Governorate name in Arabic
        public string GovCode { get; set; } = string.Empty; // Governorate code
        public int Country_Id { get; set; } // Foreign key referencing the Country entity

        public virtual Country? Country { get; set; } // Navigation property to the related Country entity
        public ICollection<City>? Cities { get; set; } // Navigation property for the related cities

    }

}
