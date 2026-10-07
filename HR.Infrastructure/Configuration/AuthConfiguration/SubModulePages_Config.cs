using HR.Domain.Models.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.Infrastructure.Configuration.AuthConfiguration
{
 public class SubModulePages_Config : IEntityTypeConfiguration<ModulePages>
 {
  public void Configure ( EntityTypeBuilder<ModulePages> builder )
  {
   builder.ToTable ("ModulePages","Auth");
   builder.HasKey (p => p.PageId);
   builder.Property (p => p.PageId).UseIdentityColumn (1,1).HasColumnType ("int");
   builder.Property (p => p.PageName_en).HasColumnType ("nvarchar").HasMaxLength (150).IsRequired ();
   builder.Property (p => p.PageName_ar).HasColumnType ("nvarchar").HasMaxLength (200).IsRequired ();
   builder.Property (p => p.PageUrl).HasColumnType ("nvarchar").HasMaxLength (256).IsRequired ();


   // Indexes
   //=========
   // Create a unique index on the PageName_en property to ensure that each page name is unique in the database.
   // The index is named "IX_ModulePage_PageName_en" and is created on the PageName_en column of the ModulePage table.

   builder.HasIndex (p => p.PageName_en)
       .HasDatabaseName ("IX_ModulePage_PageName_en")
       .IsUnique ();
   //===================================================================
   // Create a unique index on the PageName_ar property to ensure that each page name is unique in the database.
   // The index is named "IX_ModulePage_PageName_ar" and is created on the PageName_ar column of the ModulePage table.

   builder.HasIndex (p => p.PageName_ar)
       .HasDatabaseName ("IX_ModulePage_PageName_ar")
       .IsUnique ();
   //====================================================================
   // Relationships
   //=================


   builder.HasMany (p => p.Role_Page_Permissions)
       .WithOne (rp => rp.ModulePages)
       .HasForeignKey (rp => rp.PageId)
       .OnDelete (DeleteBehavior.Restrict);

   builder.HasMany (p => p.User_Page_Permissions)
       .WithOne (up => up.ModulePages)
       .HasForeignKey (up => up.PageId)
       .OnDelete (DeleteBehavior.Restrict);

   builder.HasMany (p => p.Module_ModulePages)
       .WithOne (mp => mp.ModulePages)
       .HasForeignKey (mp => mp.PageId)
       .OnDelete (DeleteBehavior.Restrict);

   builder.HasMany (p => p.Role_ModulePages)
       .WithOne (rmp => rmp.ModulePages)
       .HasForeignKey (rmp => rmp.PageId)
       .OnDelete (DeleteBehavior.Restrict);

  }

 }
}
