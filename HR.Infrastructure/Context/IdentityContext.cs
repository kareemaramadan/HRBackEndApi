using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Infrastructure.Context
{
    public class IdentityContext : IdentityDbContext<AppUser, AppRole, string>
    {
        private readonly IConfiguration _configuration;
        public IdentityContext(DbContextOptions<IdentityContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("IdentityConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>().ToTable("Users", "Auth");
            modelBuilder.Entity<AppRole>().ToTable("Roles", "Auth");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Auth");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Auth");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Auth");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Auth");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Auth");

        }
    }
}