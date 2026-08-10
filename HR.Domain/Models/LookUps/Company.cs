namespace HR.Domain.Models.LookUps
{
    public class Company
    {

        public int Id { get; set; } // Primary key for the Company entity
        public string CompName_en { get; set; }= string.Empty; // Company name in English
        public string CompName_ar { get; set; } = string.Empty; // Company name in Arabic
        public int Country_Id { get; set; } // Foreign key referencing the Country entity
        public int Gov_Id { get; set; } // Foreign key referencing the Governorate entity
        public int City_Id { get; set; } // Foreign key referencing the City entity
        public string Address_en { get; set; } = string.Empty; // Company address in English
        public string Address_ar { get; set; } = string.Empty; // Company address in Arabic
        public byte[] CompLogo { get; set; } = Array.Empty<byte>(); // Company logo as a byte array


        public virtual Country? Country { get; set; } // Navigation property for the related Country entity
        public virtual Governorate? Governorate { get; set; } // Navigation property for the related Governorate entity
        public virtual City? City { get; set; }  // Navigation property for the related City entity

    }
}
