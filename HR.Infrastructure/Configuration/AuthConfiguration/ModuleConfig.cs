using HR.Domain.Models.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Infrastructure.Configuration.AuthConfiguration
{
    public class ModuleConfig : IEntityTypeConfiguration<Module>
    {
        public void Configure ( EntityTypeBuilder<Module> builder )
        {
            builder.ToTable ( "Modules", "Auth" );
            builder.HasKey ( p => p.ModuleId );
            builder.Property ( p => p.ModuleId ).UseIdentityColumn ( 1, 1 ).HasColumnType ( "int" );
            builder.Property ( p => p.ModuleName ).HasColumnType ( "nvarchar" ).HasMaxLength ( 150 ).IsRequired ( );
            builder.Property ( p => p.ModuleImage ).HasColumnType ( "varbinary" ).HasMaxLength ( 256 );


            // Indexes
            //=========
            // Create a unique index on the ModuleName property to ensure that each module name is unique in the database.
            // The index is named "IX_Module_ModuleName" and is created on the ModuleName column of the Module table.

            builder.HasIndex ( p => p.ModuleName )
                .HasDatabaseName ( "IX_Module_ModuleName" )
                .IsUnique ( );

            //====================================================================
            // Relationships
            //=================
            // Define a one-to-many relationship between Module and ModulePage entities.
            // Each Module can have multiple ModulePages, and each ModulePage is associated with one Module.
            // The foreign key in the ModulePage entity is ModuleId, and when a Module is deleted, all associated ModulePages will also be deleted (cascade delete).

            builder.HasMany ( p => p.ModulePages )
                .WithOne ( mp => mp.Modules )
                .HasForeignKey ( mp => mp.ModuleId )
                .OnDelete ( DeleteBehavior.Cascade );

        }
    }
}
