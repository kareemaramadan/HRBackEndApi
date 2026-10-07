using HR.Domain.Models.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.Infrastructure.Configuration.AuthConfiguration
{
 public class Role_Page_Permissions_Config : IEntityTypeConfiguration<Role_Page_Permissions>
 {
  public void Configure ( EntityTypeBuilder<Role_Page_Permissions> builder )
  {
   builder.ToTable ("Role_Page_Permissions","Auth");
   builder.HasKey (p => new { p.RoleId,p.PageId,p.PermissionId });
   builder.Property (p => p.RoleId)
   .HasMaxLength (450)
   .HasColumnType ("nvarchar(450)")
   .IsRequired ();
   builder.Property (p => p.PageId)
    .HasColumnType ("int").IsRequired ();
   builder.Property (p => p.PermissionId)
    .HasColumnType ("int")
    .IsRequired ();


   // Indexes
   //=========


   builder.HasIndex (p => new { p.RoleId,p.PageId,p.PermissionId })
       .HasDatabaseName ("IX_RolePagePermission_RoleId")
       .IsUnique ();



   //====================================================================
   // Relationships
   //=================
   // Define a one-to-many relationship between Roles and RolePagePermission entities.
   // Each Role can have multiple RolePagePermissions, and each RolePagePermission is associated with one Role.
   // The foreign key in the RolePagePermission entity is RoleId, and when a Role is deleted, all associated RolePagePermissions will also be deleted (cascade delete).

   builder.HasOne (p => p.Role)
       .WithMany (r => r.Role_Page_Permissions)
       .HasForeignKey (p => p.RoleId)
       .OnDelete (DeleteBehavior.Restrict);

   // Define a one-to-many relationship between ModulePage and RolePagePermission entities.
   // Each ModulePage can have multiple RolePagePermissions, and each RolePagePermission is associated with one ModulePage.
   // The foreign key in the RolePagePermission entity is PageId, and when a ModulePage is deleted, all associated RolePagePermissions will also be deleted (cascade delete).

   builder.HasOne (p => p.SubModulePage)
       .WithMany (m => m.Role_Page_Permissions)
       .HasForeignKey (p => p.PageId)
       .OnDelete (DeleteBehavior.Restrict);

   // Define a one-to-many relationship between Permission and RolePagePermission entities.
   // Each Permission can have multiple RolePagePermissions, and each RolePagePermission is associated with one Permission.
   // The foreign key in the RolePagePermission entity is PermissionId, and when a Permission is deleted, all associated RolePagePermissions will also be deleted (cascade delete).
   builder.HasOne (p => p.Permission)
       .WithMany (p => p.Role_Page_Permissions)
       .HasForeignKey (p => p.PermissionId)
       .OnDelete (DeleteBehavior.Restrict);


  }

 }
}
