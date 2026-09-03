using HR.Domain.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Infrastructure.Configuration.AuthConfiguration
{
    public class UserConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure ( EntityTypeBuilder<AppUser> builder )
        {
            builder.HasMany(u => u.UserPagePermissions )
                .WithOne ( up => up.Users )
                .HasForeignKey ( up => up.UserId )
                .OnDelete ( DeleteBehavior.Cascade );
        }
    }
}
