using HR.Domain.Models.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Infrastructure.Configuration.AuthConfiguration
{
    public class UserPagePermissionsConfig : IEntityTypeConfiguration<UserPagePermission>
    {
        public void Configure ( EntityTypeBuilder<UserPagePermission> builder )
        {
            builder.ToTable ( "UserPagePermissions", "Auth" );
            builder.HasKey ( p => new { p.UserId, p.PageId, p.PermissionId } );
            builder.Property ( p => p.IsAllowed ).HasColumnType ( "bit" ).IsRequired ( );

            // Indexes
            //=========
            // Create a unique index on the UserId property to ensure that each user ID is unique in the database.
            // The index is named "IX_UserPagePermission_UserId" and is created on the UserId column of the UserPagePermission table.

            builder.HasIndex ( p => p.UserId )
                .HasDatabaseName ( "IX_UserPagePermission_UserId" )
                .IsUnique ( );

            // Create a unique index on the PageId property to ensure that each page ID is unique in the database.
            // The index is named "IX_UserPagePermission_PageId" and is created on the PageId column of the UserPagePermission table.

            builder.HasIndex ( p => p.PageId )
                .HasDatabaseName ( "IX_UserPagePermission_PageId" )
                .IsUnique ( );

            //====================================================================
            // Relationships
            //=================
            // Define a one-to-many relationship between Users and RolePagePermission entities.
            // Each User can have multiple RolePagePermissions, and each RolePagePermission is associated with one User.
            // The foreign key in the RolePagePermission entity is UserId, and when a User is deleted, all associated UserPagePermissions will also be deleted (cascade delete).

            builder.HasOne ( u => u.Users )
                .WithMany ( u => u.UserPagePermissions )
                .HasForeignKey ( u => u.UserId )
                .OnDelete ( DeleteBehavior.Cascade );

            //  // Define a one-to-many relationship between ModulePage and UserPagePermission entities.
            // Each ModulePage can have multiple UserPagePermissions, and each UserPagePermission is associated with one ModulePage.
            // The foreign key in the UserPagePermission entity is PageId, and when a ModulePage is deleted, all associated UserPagePermissions will also be deleted (cascade delete).
            builder.HasOne(u=> u.Pages)
                .WithMany(u=> u.UserPagePermissions)
                .HasForeignKey( u => u.PageId )
                .OnDelete( DeleteBehavior.Cascade );


            // Define a one-to-many relationship between Permission and UserPagePermission entities.
            // Each Permission can have multiple UserPagePermissions, and each UserPagePermission is associated with one Permission.
            // The foreign key in the UserPagePermission entity is PermissionId, and when a Permission is deleted, all associated UserPagePermissions will also be deleted (cascade delete).

            builder.HasOne ( u => u.Permissions )
                .WithMany ( u => u.UserPagePermissions )
                .HasForeignKey ( u => u.PermissionId )
                .OnDelete ( DeleteBehavior.Cascade );




        }
    }
}
