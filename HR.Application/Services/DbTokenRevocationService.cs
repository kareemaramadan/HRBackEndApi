using HR.Application.Interfaces;
using HR.Domain.Models.Authorization;

namespace HR.Application.Services
{
    public class DbTokenRevocationService(IBaseRepository<RevokedToken> repo) : ITokenRevocationService
    {
        public async Task<bool> IsRevokedAsync(string jti)
        {
            if (string.IsNullOrWhiteSpace(jti)) return true;
            // use count to avoid pulling entities
            var count = await repo.CountAsync(r => r.Jti == jti.Trim());
            return count > 0;
        }

        public async Task RevokeAsync(string jti, DateTime? expiresAtUtc = null)
        {
            if (string.IsNullOrWhiteSpace(jti)) return;
            var normalized = jti.Trim();
            var existing = (await repo.FindAsync(r => r.Jti == normalized)).FirstOrDefault();
            if (existing is null)
            {
                await repo.CreateAsync(new RevokedToken
                {
                    Jti = normalized,
                    RevokedAtUtc = DateTime.UtcNow,
                    ExpiresAtUtc = expiresAtUtc
                });
            }
        }
    }
}