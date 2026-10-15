using HR.Domain.Models.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.Infrastructure.Configuration.AuthConfiguration
{
 public class RevokedToken_Config : IEntityTypeConfiguration<RevokedToken>
 {
  public void Configure ( EntityTypeBuilder<RevokedToken> builder )
  {
   builder.ToTable ("RevokedTokens","Auth");
   builder.HasKey (T => T.Id);
   builder.Property (T => T.Id).UseIdentityColumn (1,1).HasColumnType ("int");
   builder.Property (T => T.Jti).HasColumnType ("nvarchar").HasMaxLength (150).IsRequired ();
   builder.Property (T => T.ExpiresAtUtc).HasColumnType ("DateTime").IsRequired ();
   builder.Property (T => T.RevokedAtUtc).HasColumnType ("DateTime").IsRequired ();


   builder.HasIndex (T => T.Jti).HasDatabaseName ("IX_RevokedToken_Jti").IsUnique ();

  }
 }
}
