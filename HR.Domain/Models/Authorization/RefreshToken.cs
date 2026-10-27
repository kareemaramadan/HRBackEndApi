using HR.Domain.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace HR.Domain.Models.Authorization
{
 public class RefreshToken
 {
  public int Id { get; set; } // Database primary key.
  [Required]
  public string UserId { get; set; } = string.Empty; // Identity user who owns this token.
  [Required]
  public string TokenRefreshed { get; set; } = string.Empty; // SHA-256 hash, never the raw refresh token.
  public DateTime ExpiresAtUtc { get; set; } // Expiration time in UTC.
  public DateTime CreatedAtUtc { get; set; } // Creation time in UTC.
  public DateTime? RevokedOn { get; set; }
  public bool IsExpired => DateTime.UtcNow>=ExpiresAtUtc;
  public bool IsActive => RevokedOn==null&&!IsExpired;
  public virtual AppUser User { get; set; } = null!; // Navigation to the owning user.

 }
}