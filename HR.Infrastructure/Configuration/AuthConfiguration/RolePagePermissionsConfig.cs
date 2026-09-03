using HR.Domain.Models.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Infrastructure.Configuration.AuthConfiguration
{
    public class RolePagePermissionsConfig : IEntityTypeConfiguration<RolePagePermission>
    {
        public void Configure ( EntityTypeBuilder<RolePagePermission> builder )
        {
            builder.ToTable ( "RolePagePermissions", "Auth" );
            builder.HasKey ( p => new { p.RoleId, p.PageId, p.PermissionId } );


            // Indexes
            //=========
            // Create a unique index on the ModuleName property to ensure that each module name is unique in the database.
            // The index is named "IX_Module_ModuleName" and is created on the ModuleName column of the Module table.

            builder.HasIndex ( p => p.RoleId )
                .HasDatabaseName ( "IX_RolePagePermission_RoleId" )
                .IsUnique ( );

            // Create a unique index on the PageId property to ensure that each page ID is unique in the database.
            // The index is named "IX_RolePagePermission_PageId" and is created on the PageId column of the RolePagePermission table.
            builder.HasIndex ( p => p.PageId )
                .HasDatabaseName ( "IX_RolePagePermission_PageId" )
                .IsUnique ( );

            //====================================================================
            // Relationships
            //=================
            // Define a one-to-many relationship between Roles and RolePagePermission entities.
            // Each Role can have multiple RolePagePermissions, and each RolePagePermission is associated with one Role.
            // The foreign key in the RolePagePermission entity is RoleId, and when a Role is deleted, all associated RolePagePermissions will also be deleted (cascade delete).

            builder.HasOne ( p => p.Roles )
                .WithMany ( r => r.RolePagePermissions )
                .HasForeignKey ( p => p.RoleId )
                .OnDelete ( DeleteBehavior.Cascade );

            // Define a one-to-many relationship between ModulePage and RolePagePermission entities.
            // Each ModulePage can have multiple RolePagePermissions, and each RolePagePermission is associated with one ModulePage.
            // The foreign key in the RolePagePermission entity is PageId, and when a ModulePage is deleted, all associated RolePagePermissions will also be deleted (cascade delete).

            builder.HasOne ( p => p.ModulePages )
                .WithMany ( m => m.RolePagePermissions )
                .HasForeignKey ( p => p.PageId )
                .OnDelete ( DeleteBehavior.Cascade );

            // Define a one-to-many relationship between Permission and RolePagePermission entities.
            // Each Permission can have multiple RolePagePermissions, and each RolePagePermission is associated with one Permission.
            // The foreign key in the RolePagePermission entity is PermissionId, and when a Permission is deleted, all associated RolePagePermissions will also be deleted (cascade delete).
            builder.HasOne ( p => p.Permissions )
                .WithMany ( p => p.RolePagePermissions )
                .HasForeignKey ( p => p.PermissionId )
                .OnDelete ( DeleteBehavior.Cascade );


        }

    }
}
