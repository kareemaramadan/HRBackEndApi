using HR.Domain.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Infrastructure.Configuration.AuthConfiguration
{
    public class RoleConfig : IEntityTypeConfiguration<AppRole>
    {
        public void Configure ( EntityTypeBuilder<AppRole> builder )
        {
            builder.HasMany ( u => u.RolePagePermissions )
                .WithOne ( up => up.Roles )
                .HasForeignKey ( up => up.RoleId )
                .OnDelete ( DeleteBehavior.Cascade );
        }
    }

}
