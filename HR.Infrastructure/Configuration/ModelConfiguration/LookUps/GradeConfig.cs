using HR.Domain.Models.LookUps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.Infrastructure.Configuration.ModelConfiguration.LookUps
{
    public class GradeConfig:IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.ToTable("Grade", "LookUps");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).UseIdentityColumn(1, 1).HasColumnType("int");
            builder.Property(c => c.GradeName_en).HasColumnType("nvarchar").HasMaxLength(150).IsRequired();
            builder.Property(c => c.GradeName_ar).HasColumnType("nvarchar").HasMaxLength(150).IsRequired();
            builder.Property(c => c.priority).HasColumnType("int").IsRequired();
            builder.Property(c => c.percentage).HasColumnType("int").IsRequired();

            // Indexes
            //=========
            // Define unique indexes for GradeName_en and GradeName_ar to ensure no duplicate city name exist in either language.

            // Unique index on GradeName_en
            builder.HasIndex(c => c.GradeName_en)
                .HasDatabaseName("IX_Grade_GradeName_en")
                .IsUnique();

            // Unique index on GradeName_ar
            builder.HasIndex(c => c.GradeName_ar)
                .HasDatabaseName("IX_Grade_GradeName_ar")
                .IsUnique();
        }
    }
}
