using HR.Domain.Models.LookUps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.Infrastructure.Configuration.ModelConfiguration.LookUps
{
    public class CityConfig : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {

            builder.ToTable("City", "LookUps");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).UseIdentityColumn(1, 1).HasColumnType("int");
            builder.Property(c => c.CityName_en).HasColumnType("nvarchar").HasMaxLength(150).IsRequired();
            builder.Property(c => c.CityName_ar).HasColumnType("nvarchar").HasMaxLength(150).IsRequired();
            builder.Property(c => c.Gov_Id).HasColumnType("int").IsRequired();


            // Indexes
            //=========
            // Define unique indexes for CityName_en and CityName_ar to ensure no duplicate city name exist in either language.

            // Unique index on CityName_en
            builder.HasIndex(c => c.CityName_en)
                .HasDatabaseName("IX_City_CityName_en")
                .IsUnique();

            // Unique index on CityName_ar
            builder.HasIndex(c => c.CityName_ar)
                .HasDatabaseName("IX_City_CityName_ar")
                .IsUnique();
            //====================================================================
            // Relationships
            //=================
            // Configure the relationship between City and Governorate entities, specifying that a city belongs to one governorate,
            // and a governorate can have many cities. The foreign key is Gov_Id,
            // and the delete behavior is set to restrict to prevent deletion of a governorate if it has associated cities.

            // Configure the relationship between City and Governorate

            builder.HasOne(ci => ci.Governorate)
                   .WithMany(g => g.Cities)
                   .HasForeignKey(ci => ci.Gov_Id)
                   .OnDelete(DeleteBehavior.Restrict);
            //====================================================================
            // Configure the relationship between Company and City entities, specifying that a city belongs to one company,
            // and a city can have many companies. The foreign key is Company_Id,
            // and the delete behavior is set to restrict to prevent deletion of a company if it has associated cities.

            // Configure the relationship between City and Company

            builder.HasMany(ci => ci.Companies)
                .WithOne(comp => comp.City)
                .HasForeignKey(comp => comp.City_Id)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
