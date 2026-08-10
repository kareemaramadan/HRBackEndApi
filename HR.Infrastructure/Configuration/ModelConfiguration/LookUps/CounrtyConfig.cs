using HR.Domain.Models.LookUps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.Infrastructure.Configuration.ModelConfiguration.LookUps
{
    /// <summary>
    /// Configuration class for the Country entity, 
    /// implementing IEntityTypeConfiguration<Country> to define the entity's schema and relationships in the database.
    /// </summary>
    public class CountryConfig : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Country", "LookUps");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).UseIdentityColumn(1, 1).HasColumnType("int");
            builder.Property(c => c.CountryName_en).HasColumnType("nvarchar").HasMaxLength(150).IsRequired();
            builder.Property(c => c.CountryName_ar).HasColumnType("nvarchar").HasMaxLength(150).IsRequired();

            //Indexes
            //=======
            // Define unique indexes for CountryName_en and CountryName_ar to ensure no duplicate country names exist in either language.

            // Unique index on CountryName_en
            builder.HasIndex(c => c.CountryName_en)
                .HasDatabaseName("IX_Country_Country_en")
                .IsUnique();
            // Unique index on CountryName_ar
            builder.HasIndex(c => c.CountryName_ar)
                .HasDatabaseName("IX_Country_Country_ar")
                .IsUnique();


            // Relationships
            //==============
            //====================================================================
            // Configure the relationship between Country and Company entities, specifying that a company belongs to one Country,
            // and a Country can have many companies. The foreign key is Country_Id,
            // and the delete behavior is set to restrict to prevent deletion of a Country if it has associated companies.

            builder.HasMany(c => c.Companies)
                .WithOne(comp => comp.Country)
                .HasForeignKey(comp => comp.Country_Id)
                .OnDelete(DeleteBehavior.Restrict);
            //====================================================================
            // Configure the relationship between Country and Governorates entities, specifying that a Governorate belongs to one Country,
            // and a Country can have many Governorates. The foreign key is Country_Id,
            // and the delete behavior is set to restrict to prevent deletion of a Country if it has associated Governorates.

            builder.HasMany(c => c.Governorates)
                .WithOne(g => g.Country)
                .HasForeignKey(g => g.Country_Id)
                .OnDelete(DeleteBehavior.Restrict);
            //====================================================================
            // Configure the relationship between Country and City entities, specifying that a City belongs to one Country,
            // and a Country can have many Cities. The foreign key is Country_Id,
            // and the delete behavior is set to restrict to prevent deletion of a Country if it has associated Cities.

            builder.HasMany(c=>c.Cities)
                .WithOne(ci => ci.Country)
                .HasForeignKey(ci => ci.Country_Id)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
