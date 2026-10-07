using HR.Domain.Models.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.Infrastructure.Configuration.AuthConfiguration
{
 public class User_Page_Permissions_Config : IEntityTypeConfiguration<User_Page_Permission>
 {
  public void Configure ( EntityTypeBuilder<User_Page_Permission> builder )
  {
   builder.ToTable ("UserPagePermissions","Auth");
   builder.HasKey (p => new { p.UserId,p.PageId,p.PermissionId });
   builder.Property (p => p.IsAllowed).HasColumnType ("bit").IsRequired ();

   // Indexes
   //=========

   builder.HasIndex (p => new { p.UserId,p.PageId,p.PermissionId })
    .HasDatabaseName ("IX_UserPagePermissions_Unique")
    .IsUnique ();

   //====================================================================
   // Relationships
   //=================
   // Define a one-to-many relationship between Users and RolePagePermission entities.
   // Each User can have multiple RolePagePermissions, and each RolePagePermission is associated with one User.
   // The foreign key in the RolePagePermission entity is UserId, and when a User is deleted, all associated UserPagePermissions will also be deleted (cascade delete).

   builder.HasOne (u => u.Users)
                .WithMany (u => u.User_Page_Permissions)
                .HasForeignKey (u => u.UserId)
                .OnDelete (DeleteBehavior.Restrict);

   //  // Define a one-to-many relationship between ModulePage and UserPagePermission entities.
   // Each ModulePage can have multiple UserPagePermissions, and each UserPagePermission is associated with one ModulePage.
   // The foreign key in the UserPagePermission entity is PageId, and when a ModulePage is deleted, all associated UserPagePermissions will also be deleted (cascade delete).
   builder.HasOne (u => u.ModulePages)
       .WithMany (u => u.User_Page_Permissions)
       .HasForeignKey (u => u.PageId)
       .OnDelete (DeleteBehavior.Restrict);


   // Define a one-to-many relationship between Permission and UserPagePermission entities.
   // Each Permission can have multiple UserPagePermissions, and each UserPagePermission is associated with one Permission.
   // The foreign key in the UserPagePermission entity is PermissionId, and when a Permission is deleted, all associated UserPagePermissions will also be deleted (cascade delete).

   builder.HasOne (u => u.Permissions)
       .WithMany (u => u.User_Page_Permissions)
       .HasForeignKey (u => u.PermissionId)
       .OnDelete (DeleteBehavior.Restrict);




  }
 }
}
