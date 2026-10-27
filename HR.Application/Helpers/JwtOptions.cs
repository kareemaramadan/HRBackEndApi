using System.ComponentModel.DataAnnotations;

namespace HR.Application.Helpers
{

 public class JwtOptions
 {
  [Required]
  public string? Key { get; set; }
  [Required]
  public string? Issuer { get; set; }
  [Required]
  public string? Audience { get; set; }
  [Required]
  public double? ExpireAt { get; set; }
  [Required]
  public double? RefreshExpiresAt { get; set; }

 }
}
