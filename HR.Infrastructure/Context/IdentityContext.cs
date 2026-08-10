
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
namespace HR.Infrastructure.Context
{
    public class IdentityContext(DbContextOptions<IdentityContext> options, IConfiguration configuration) : IdentityDbContext<AppUser,AppRole,string>(options)
    {
        private readonly IConfiguration _configuration = configuration;

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
