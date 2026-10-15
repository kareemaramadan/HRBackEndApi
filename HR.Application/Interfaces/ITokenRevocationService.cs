using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Application.Interfaces
{
    public interface ITokenRevocationService
    {
        Task<bool> IsRevokedAsync(string jti);
        Task RevokeAsync(string jti, DateTime? expiresAtUtc = null);
    }
}
