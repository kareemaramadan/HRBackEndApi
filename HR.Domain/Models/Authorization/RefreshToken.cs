using HR.Domain.Models.Identity;

namespace HR.Domain.Models.Authorization
{
 public class RefreshToken
 {
  public int Id { get; set; } // Database primary key.

  public string UserId { get; set; } = string.Empty; // Identity user who owns this token.
  public string TokenHash { get; set; } = string.Empty; // SHA-256 hash, never the raw refresh token.
  public DateTime ExpiresAtUtc { get; set; } // Expiration time in UTC.
  public DateTime CreatedAtUtc { get; set; } // Creation time in UTC.
  public DateTime? RevokedAtUtc { get; set; } // Non-null means this token has been revoked or rotated.


  public virtual AppUser User { get; set; } = null!; // Navigation to the owning user.
 }
}