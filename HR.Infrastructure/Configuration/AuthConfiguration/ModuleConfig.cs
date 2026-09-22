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
            builder.Property ( p => p.ModuleName_en ).HasColumnType ( "nvarchar" ).HasMaxLength ( 150 ).IsRequired ( );
            builder.Property ( p => p.ModuleName_ar ).HasColumnType ( "nvarchar" ).HasMaxLength ( 200 ).IsRequired ( );
            builder.Property ( p => p.ModuleImage ).HasColumnType ( "varbinary(MAX)" ).IsRequired ( );


            // Indexes
            //=========
            // Create a unique index on the ModuleName_en property to ensure that each module name is unique in the database.
            // The index is named "IX_Module_ModuleName_en" and is created on the ModuleName_en column of the Module table.

            builder.HasIndex ( p => p.ModuleName_en )
                .HasDatabaseName ( "IX_Module_ModuleName_en" )
                .IsUnique ( );

            //==================================================================
            // Create a unique index on the ModuleName_ar property to ensure that each module name is unique in the database.
            // The index is named "IX_Module_ModuleName_ar" and is created on the ModuleName_ar column of the Module table.

            builder.HasIndex ( p => p.ModuleName_ar )
                .HasDatabaseName ( "IX_Module_ModuleName_ar" )
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
