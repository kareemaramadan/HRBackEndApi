using HR.Domain.Models.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.Infrastructure.Configuration.AuthConfiguration
{
 public class Role_SubModules_Config : IEntityTypeConfiguration<Role_SubModules>
 {
  public void Configure ( EntityTypeBuilder<Role_SubModules> builder )
  {
   // Table & Column Mappings
   //==========================
   builder.ToTable ("Role_SubModules","Auth");
   // Primary Key
   //===============
   builder.HasKey (p => new { p.RoleId,p.ModuleId,p.SubModuleId });
   builder.Property (p => p.RoleId)
    .HasColumnName ("RoleId")
    .HasMaxLength (450)
    .HasColumnType ("nvarchar(450)")
    .IsRequired ();
   builder.Property (p => p.ModuleId)
    .HasColumnName ("ModuleId")
    .HasColumnType ("int")
    .IsRequired ();

   builder.Property (p => p.SubModuleId)
    .HasColumnName ("SubModuleId")
    .HasColumnType ("int")
    .IsRequired ();
   // Indexes
   //===============

   builder.HasIndex (p => new { p.RoleId,p.ModuleId,p.SubModuleId })
    .HasDatabaseName ("IX_RoleModules_RoleId_ModuleId_SubModuleId")
    .IsUnique ();





   // Relationships
   //================
   // RoleModules has one AppRole, with many RoleModules, foreign key is RoleId
   builder.HasOne (p => p.Role)
    .WithMany (r => r.Role_SubModules)
    .HasForeignKey (p => p.RoleId)
    .OnDelete (DeleteBehavior.Restrict);
   // RoleModules has one Module, with many RoleModules, foreign key is ModuleId
   builder.HasOne (p => p.Module)
    .WithMany (m => m.Role_SubModules)
    .HasForeignKey (p => p.ModuleId)
    .OnDelete (DeleteBehavior.Restrict);


  }
 }
}
