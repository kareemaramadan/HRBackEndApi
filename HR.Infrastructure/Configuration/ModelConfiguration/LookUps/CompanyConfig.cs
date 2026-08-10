using HR.Domain.Models.LookUps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.Infrastructure.Configuration.ModelConfiguration.LookUps
{   
    /// <summary>
    /// Configuration class for the Company entity, 
    /// implementing IEntityTypeConfiguration<Company> to define the entity's schema and relationships in the database.
    /// </summary>
    public class CompanyConfig : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Company", "LookUps");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).UseIdentityColumn(1, 1).HasColumnType("int");
            builder.Property(c => c.CompName_en).HasColumnType("nvarchar").HasMaxLength(100).IsRequired();
            builder.Property(c => c.CompName_ar).HasColumnType("nvarchar").HasMaxLength(100).IsRequired();
            builder.Property(c => c.City_Id).HasColumnType("int").IsRequired();
            builder.Property(c => c.Address_en).HasColumnType("nvarchar").HasMaxLength(200);
            builder.Property(c => c.Address_ar).HasColumnType("nvarchar").HasMaxLength(200);
            builder.Property(c => c.CompLogo).HasColumnType("image").IsRequired();
            //====================================================================
            // Indexes
            //=========
            // Define unique indexes for CompanyName_en and CompanyName_ar to ensure no duplicate company name exist in either language.

            // Unique index on CompName_en
            builder.HasIndex(c => c.CompName_en)
                .HasDatabaseName("IX_Company_CompName_en")
                .IsUnique(); 

            // Unique index on CompName_ar
            builder.HasIndex(c => c.CompName_ar)
                .HasDatabaseName("IX_Company_CompName_ar")
                .IsUnique();
            //====================================================================
            // Relationships
            //=================
            // Configure the relationship between City and Company entities, specifying that a company belongs to one City,
            // and a City can have many companies. The foreign key is City_Id,
            // and the delete behavior is set to restrict to prevent deletion of a City if it has associated companies.

            // One-to-many relationship with City
            builder.HasOne(comp => comp.City)
              .WithMany(ci => ci.Companies)
              .HasForeignKey(comp => comp.City_Id)
              .OnDelete(DeleteBehavior.Restrict);


            
          
        }
    }
}