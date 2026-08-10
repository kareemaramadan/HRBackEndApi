using HR.Domain.Models.LookUps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.Infrastructure.Configuration.ModelConfiguration.LookUps
{
    public class GovernorateConfig : IEntityTypeConfiguration<Governorate>
    {
        public void Configure(EntityTypeBuilder<Governorate> builder)
        {
            builder.ToTable("Governorate", "LookUps");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).UseIdentityColumn(1, 1).HasColumnType("int");
            builder.Property(c => c.Country_Id).HasColumnType("int").IsRequired();
            builder.Property(c => c.GovName_en).HasColumnType("nvarchar").HasMaxLength(100).IsRequired();
            builder.Property(c => c.GovName_ar).HasColumnType("nvarchar").HasMaxLength(100).IsRequired();
            builder.Property(c => c.GovCode).HasColumnType("nvarchar").HasMaxLength(5).IsRequired();
            //====================================================================
            // Indexes
            //=========
            // Define unique indexes for CompanyName_en and CompanyName_ar to ensure no duplicate company name exist in either language.

            // Unique index on CompName_en
            builder.HasIndex(c => c.GovName_en)
                .HasDatabaseName("IX_Governorate_GovName_en")
                .IsUnique();

            // Unique index on CompName_ar
            builder.HasIndex(c => c.GovName_ar)
                .HasDatabaseName("IX_Governorate_GovName_ar")
                .IsUnique();
            //====================================================================
            // Relationships
            //=================
            // Configure the relationship between Governorate and Country entities, specifying that a governorate belongs to one country,
            // and a     can have many governorates. The foreign key is Country_Id,
            // and the delete behavior is set to restrict to prevent deletion of a country if it has associated governorates.

            builder.HasOne(g => g.Country)
                .WithMany(co => co.Governorates)
                .HasForeignKey(g => g.Country_Id)
                .OnDelete(DeleteBehavior.Restrict);
            //====================================================================
            // Configure the relationship between Governorate and Company entities, specifying that a company belongs to one Governorate,
            // and a Governorate can have many companies. The foreign key is Gov_Id,
            // and the delete behavior is set to restrict to prevent deletion of a Governorate if it has associated companies.

            builder.HasMany(g => g.Companies)
                .WithOne(comp => comp.Governorate)
                .HasForeignKey(comp => comp.Gov_Id)
                .OnDelete(DeleteBehavior.Restrict);
            //====================================================================
            // Configure the relationship between City and Governorate entities, specifying that a city belongs to one governorate,
            // and a governorate can have many cities. The foreign key is Gov_Id,
            // and the delete behavior is set to restrict to prevent deletion of a governorate if it has associated cities.

            builder.HasMany(g => g.Cities)
                .WithOne(ci => ci.Governorate)
                .HasForeignKey(ci => ci.Gov_Id)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
