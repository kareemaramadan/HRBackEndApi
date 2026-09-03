using HR.Domain.Models.Authorization;
using HR.Domain.Models.LookUps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Infrastructure.Configuration.AuthConfiguration
{
    public class PermissionConfig : IEntityTypeConfiguration<Permission>
    {
        public void Configure ( EntityTypeBuilder<Permission> builder )
        {

            builder.ToTable ( "Permission", "Auth" );
            builder.HasKey ( p => p.PermissionId );
            builder.Property ( p => p.PermissionId ).UseIdentityColumn ( 1, 1 ).HasColumnType ( "int" );
            builder.Property ( p => p.PermissionName ).HasColumnType ( "nvarchar" ).HasMaxLength ( 150 ).IsRequired ( );
            builder.Property ( p => p.PermissionCode ).HasColumnType ( "nvarchar" ).HasMaxLength ( 150 ).IsRequired ( );


            // Indexes
            //=========
            // Create a unique index on the PermissionName property to ensure that each permission name is unique in the database.
            // The index is named "IX_Permission_PermissionName" and is created on the PermissionName column of the Permission table.

            builder.HasIndex ( p => p.PermissionName )
                .HasDatabaseName ( "IX_Permission_PermissionName" )
                .IsUnique ( );

            //====================================================================
            // Relationships
            //=================
            // Define a one-to-many relationship between Permission and UserPagePermission entities.
            // Each Permission can have multiple UserPagePermissions, and each UserPagePermission is associated with one Permission.
            // The foreign key in the UserPagePermission entity is PermissionId, and when a Permission is deleted, all associated UserPagePermissions will also be deleted (cascade delete).

            builder.HasMany ( p => p.UserPagePermissions )
                .WithOne ( upp => upp.Permissions )
                .HasForeignKey ( upp => upp.PermissionId )
                .OnDelete ( DeleteBehavior.Cascade );

            // Define a one-to-many relationship between Permission and RolePagePermission entities.
            // Each Permission can have multiple RolePagePermissions, and each RolePagePermission is associated with one Permission.
            // The foreign key in the RolePagePermission entity is PermissionId, and when a Permission is deleted, all associated RolePagePermissions will also be deleted (cascade delete).

            builder.HasMany ( p => p.RolePagePermissions )
                .WithOne ( upp => upp.Permissions )
                .HasForeignKey ( upp => upp.PermissionId )
                .OnDelete ( DeleteBehavior.Cascade );

        }
    }
}
