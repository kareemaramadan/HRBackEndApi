using HR.Domain.Models.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.Infrastructure.Configuration.AuthConfiguration
{
 public class Module_SubModules_Config : IEntityTypeConfiguration<Module_SubModules>
 {
  public void Configure ( EntityTypeBuilder<Module_SubModules> builder )
  {
   builder.ToTable ("Module_SubModules","Auth");
   builder.HasKey (p => new { p.ModuleId,p.SubModuleId });
   builder.Property (p => p.ModuleId).UseIdentityColumn (1,1).HasColumnType ("int");
   builder.Property (p => p.SubModuleId).UseIdentityColumn (1,1).HasColumnType ("int");


   // Indexes
   //========

   builder.HasIndex (p => new { p.ModuleId,p.SubModuleId }).HasDatabaseName ("IX_Module_SubModules_ModuleId_SubModuleId").IsUnique (true);


   //====================================================================
   // Relationships
   //=================

   builder.HasOne (p => p.Module)
         .WithMany (p => p.Module_SubModules)
         .HasForeignKey (p => p.ModuleId)
         .OnDelete (DeleteBehavior.Restrict)
         .HasConstraintName ("FK_Module_SubModules_ModuleId");

   builder.HasOne (p => p.SubModule)
      .WithMany (p => p.Module_SubModules)
          .HasForeignKey (p => p.SubModuleId)
          .OnDelete (DeleteBehavior.Restrict)
          .HasConstraintName ("FK_Module_SubModules_SubModuleId");
  }
 }
}
