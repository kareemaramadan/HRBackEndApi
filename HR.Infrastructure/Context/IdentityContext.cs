using HR.Domain.Models.Authorization;
using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HR.Infrastructure.Context
{
 public class IdentityContext ( DbContextOptions<IdentityContext> options,IConfiguration configuration ) : IdentityDbContext<AppUser,AppRole,string> (options)
 {
  private readonly IConfiguration _configuration = configuration;

  protected override void OnConfiguring ( DbContextOptionsBuilder optionsBuilder )
  {
   if (!optionsBuilder.IsConfigured)
   {
    var connectionString = _configuration.GetConnectionString ("IdentityConnection");
    optionsBuilder.UseSqlServer (connectionString)
    .EnableSensitiveDataLogging (sensitiveDataLoggingEnabled: true)
    .LogTo (Console.WriteLine,LogLevel.Information);
   }
  }
  protected override void OnModelCreating ( ModelBuilder modelBuilder )
  {

   base.OnModelCreating (modelBuilder);

   modelBuilder.Entity<AppUser> ().ToTable ("Users","Auth");
   modelBuilder.Entity<AppRole> ().ToTable ("Roles","Auth");
   modelBuilder.Entity<IdentityUserRole<string>> ().ToTable ("UserRoles","Auth");
   modelBuilder.Entity<IdentityUserClaim<string>> ().ToTable ("UserClaims","Auth");
   modelBuilder.Entity<IdentityUserLogin<string>> ().ToTable ("UserLogins","Auth");
   modelBuilder.Entity<IdentityRoleClaim<string>> ().ToTable ("RoleClaims","Auth");
   modelBuilder.Entity<IdentityUserToken<string>> ().ToTable ("UserTokens","Auth");

   modelBuilder.Entity<Permission> ().ToTable ("Permissions","Auth");
   modelBuilder.Entity<Module> ().ToTable ("Modules","Auth");
   modelBuilder.Entity<SubModulePages> ().ToTable ("SubModulePages","Auth");
   modelBuilder.Entity<Role_Page_Permissions> ().ToTable ("Role_Page_Permissions","Auth");
   modelBuilder.Entity<User_Page_Permissions> ().ToTable ("User_Page_Permissions","Auth");
   modelBuilder.Entity<Module_SubModules> ().ToTable ("Module_SubModules","Auth");
   modelBuilder.Entity<Role_SubModulePages> ().ToTable ("Role_SubModulePages","Auth");
   modelBuilder.Entity<Role_SubModules> ().ToTable ("Role_SubModules","Auth");
   modelBuilder.Entity<SubModules> ().ToTable ("SubModules","Auth");


   //modelBuilder.ApplyConfigurationsFromAssembly ( typeof ( ModuleConfig ).Assembly );
   //modelBuilder.ApplyConfigurationsFromAssembly ( typeof ( ModulePagesConfig ).Assembly );
   //modelBuilder.ApplyConfigurationsFromAssembly ( typeof ( PermissionConfig ).Assembly );
   //modelBuilder.ApplyConfigurationsFromAssembly ( typeof ( RoleConfig ).Assembly );
   //modelBuilder.ApplyConfigurationsFromAssembly ( typeof ( RolePagePermissionsConfig ).Assembly );
   //modelBuilder.ApplyConfigurationsFromAssembly ( typeof ( UserConfig ).Assembly );
   //modelBuilder.ApplyConfigurationsFromAssembly ( typeof ( UserPagePermissionsConfig ).Assembly );
   //modelBuilder.ApplyConfigurationsFromAssembly ( typeof ( IdentityContext ).Assembly );

   modelBuilder.ApplyConfigurationsFromAssembly (typeof (IdentityContext).Assembly);

  }
 }
}