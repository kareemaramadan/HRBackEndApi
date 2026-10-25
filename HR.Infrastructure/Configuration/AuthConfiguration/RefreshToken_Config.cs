using HR.Domain.Models.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.Infrastructure.Configuration.AuthConfiguration
{
 public class RefreshToken_Config : IEntityTypeConfiguration<RefreshToken>
 {
  public void Configure ( EntityTypeBuilder<RefreshToken> builder )
  {
   builder.ToTable ("RefreshTokens","Auth");
   builder.HasKey (T => T.Id);
   builder.Property (T => T.Id).UseIdentityColumn (1,1).HasColumnType ("int");
   builder.Property (T => T.UserId).HasColumnType ("nvarchar").HasMaxLength (450).IsRequired ();
   builder.Property (T => T.TokenHash).HasColumnType ("nvarchar").HasMaxLength (4000).IsRequired ();
   builder.Property (T => T.ExpiresAtUtc).HasColumnType ("DateTime");
   builder.Property (T => T.CreatedAtUtc).HasColumnType ("DateTime");
   builder.Property (T => T.RevokedAtUtc).HasColumnType ("DateTime");


   builder.HasIndex (T => T.ExpiresAtUtc).HasDatabaseName ("IX_RefreshedTokens_Expires").IsUnique ();

   builder.HasOne (T => T.User)
    .WithMany (up => up.RefreshTokens)
    .HasForeignKey (T => T.UserId)
    .OnDelete (DeleteBehavior.Restrict);

  }
 }
}
