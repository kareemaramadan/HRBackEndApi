using HR.Domain.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.Infrastructure.Configuration.AuthConfiguration
{
 public class Role_Config : IEntityTypeConfiguration<AppRole>
 {
  public void Configure ( EntityTypeBuilder<AppRole> builder )
  {
   builder.HasMany (u => u.Role_Page_Permissions)
       .WithOne (up => up.Role)
       .HasForeignKey (up => up.RoleId)
       .OnDelete (DeleteBehavior.Restrict);

   builder.HasMany (u => u.Role_Modules)
       .WithOne (up => up.Role)
       .HasForeignKey (up => up.RoleId)
       .OnDelete (DeleteBehavior.Restrict);

   builder.HasMany (u => u.Role_SubModulePages)
       .WithOne (up => up.Role)
       .HasForeignKey (up => up.RoleId)
       .OnDelete (DeleteBehavior.Restrict);




  }
 }

}
