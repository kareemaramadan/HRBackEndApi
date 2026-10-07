using HR.Domain.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.Infrastructure.Configuration.AuthConfiguration
{
 public class User_Config : IEntityTypeConfiguration<AppUser>
 {
  public void Configure ( EntityTypeBuilder<AppUser> builder )
  {
   builder.HasMany (u => u.User_Page_Permissions)
       .WithOne (up => up.User)
       .HasForeignKey (up => up.UserId)
       .OnDelete (DeleteBehavior.Restrict);
  }
 }
}
