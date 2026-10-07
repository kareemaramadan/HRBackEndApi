using HR.Domain.Models.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.Infrastructure.Configuration.AuthConfiguration
{
 public class Role_SubModulePages_Config : IEntityTypeConfiguration<Role_SubModulePages>
 {
  public void Configure ( EntityTypeBuilder<Role_SubModulePages> builder )
  {
   //table Configuration
   //====================
   builder.ToTable ("Role_SubModulePages","Auth");
   builder.HasKey (p => new { p.RoleId,p.PageId });
   builder.Property (p => p.RoleId).HasMaxLength (450).IsRequired ();
   builder.Property (p => p.PageId).HasColumnType ("int").IsRequired ();

   //index
   //=====
   builder.HasIndex (p => new { p.RoleId,p.PageId })
    .HasDatabaseName ("IX_Role_ModulePages_RoleId_PageId")
    .IsUnique ();

   //relationships
   //==============
   builder.HasOne (p => p.Role)
       .WithMany (r => r.Role_SubModulePages)
       .HasForeignKey (p => p.RoleId)
       .OnDelete (DeleteBehavior.Restrict);

   builder.HasOne (p => p.SubModulePage)
       .WithMany (m => m.Role_SubModulePages)
       .HasForeignKey (p => p.PageId)
       .OnDelete (DeleteBehavior.Restrict);



  }
 }
}
