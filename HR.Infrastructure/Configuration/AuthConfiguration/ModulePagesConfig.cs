using HR.Domain.Models.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Infrastructure.Configuration.AuthConfiguration
{
    public class ModulePagesConfig : IEntityTypeConfiguration<ModulePage>
    {
        public void Configure ( EntityTypeBuilder<ModulePage> builder )
        {
            builder.ToTable ( "ModulePages", "Auth" );
            builder.HasKey ( p => p.PageId );
            builder.Property ( p => p.PageId ).UseIdentityColumn ( 1, 1 ).HasColumnType ( "int" );
            builder.Property ( p => p.PageName ).HasColumnType ( "nvarchar" ).HasMaxLength ( 150 ).IsRequired ( );
            builder.Property ( p => p.PageUrl ).HasColumnType ( "nvarchar" ).HasMaxLength ( 256 ).IsRequired ( );


            // Indexes
            //=========
            // Create a unique index on the PageName property to ensure that each page name is unique in the database.
            // The index is named "IX_ModulePage_PageName" and is created on the PageName column of the ModulePage table.

            builder.HasIndex ( p => p.PageName )
                .HasDatabaseName ( "IX_ModulePage_PageName" )
                .IsUnique ( );

            //====================================================================
            // Relationships
            //=================
            // Define a one-to-many relationship between Module and ModulePage entities.
            // Each Module can have multiple ModulePages, and each ModulePage is associated with one Module.
            // The foreign key in the ModulePage entity is ModuleId, and when a Module is deleted, all associated ModulePages will also be deleted (cascade delete).

            builder.HasOne ( p => p.Modules )
                .WithMany ( m => m.ModulePages )
                .HasForeignKey ( p => p.ModuleId )
                .OnDelete ( DeleteBehavior.Cascade );

        }

    }
}
