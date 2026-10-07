using HR.Domain.Models.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.Infrastructure.Configuration.AuthConfiguration
{
 public class SubModules_Config : IEntityTypeConfiguration<SubModules>
 {
  public void Configure ( EntityTypeBuilder<SubModules> builder )
  {
   builder.ToTable ("SubModules","Auth");
   builder.HasKey (x => x.SubModuleId);
   builder.Property (x => x.SubModuleId).HasColumnName ("SubModuleId").UseIdentityColumn (1,1).HasColumnType ("int");
   builder.Property (x => x.SubModuleName_en).HasColumnName ("SubModuleName_en").HasMaxLength (250).IsRequired ();
   builder.Property (x => x.SubModuleName_ar).HasColumnName ("SubModuleName_ar").HasMaxLength (350).IsRequired ();



   //Indexes
   //=========

   builder.HasIndex (x => x.SubModuleName_en).HasDatabaseName ("IX_SubModules_SubModuleName_en").IsUnique ();
   builder.HasIndex (x => x.SubModuleName_ar).HasDatabaseName ("IX_SubModules_SubModuleName_ar").IsUnique ();

   //Relationships
   //=============


   builder.HasMany (x => x.Module_SubModules)
    .WithOne (x => x.SubModule)
    .HasForeignKey (x => x.SubModuleId)
    .OnDelete (DeleteBehavior.Restrict);

   builder.HasMany (x => x.SubModulePages)
    .WithOne (x => x.SubModule)
    .HasForeignKey (x => x.SubModuleId)
    .OnDelete (DeleteBehavior.Restrict);



  }
 }
}
