namespace HR.Domain.Models.Authorization
{
 public class RevokedToken
 {
  public int Id { get; set; }
  public string Jti { get; set; } = null!;
  public DateTime RevokedAtUtc { get; set; }
  public DateTime? ExpiresAtUtc { get; set; } // optional to prune old tokens
 }
}