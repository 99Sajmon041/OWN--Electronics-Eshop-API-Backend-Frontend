using System.IdentityModel.Tokens.Jwt;

namespace ElectronicsEshop.Blazor.Services.Auth;

public sealed class TokenExpiryService
{
    private Timer? preExpiryTimer;
    private Timer? expiryTimer;

    public void Schedule(JwtSecurityToken jwtToken, Func<Task> onPreExpiryAsync, Func<Task> onExpiredAsync, TimeSpan? preExpiryOffset = null)
    {
        Cancel();

        string? expValue = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;

        if (string.IsNullOrWhiteSpace(expValue))
            return;

        if (!long.TryParse(expValue, out long expUnixSeconds))
            return;

        DateTimeOffset expiresAtUtc = DateTimeOffset.FromUnixTimeSeconds(expUnixSeconds);
        TimeSpan timeUntilExpiry = expiresAtUtc - DateTimeOffset.UtcNow;

        if (timeUntilExpiry <= TimeSpan.Zero)
        {
            onExpiredAsync.Invoke();
            return;
        }

        TimeSpan offset = preExpiryOffset ?? TimeSpan.FromSeconds(60);
        TimeSpan timeUntilPreExpiry = timeUntilExpiry - offset;

        if(timeUntilPreExpiry > TimeSpan.Zero)
        {
            preExpiryTimer = new Timer(
                callback: async _ => await onPreExpiryAsync.Invoke(),
                state: null,
                dueTime: timeUntilPreExpiry,
                period: Timeout.InfiniteTimeSpan
            );
        }
        else
        {
            onPreExpiryAsync.Invoke();
        }

        expiryTimer = new Timer(
            callback: async _ => await onExpiredAsync.Invoke(),
            state: null,
            dueTime: timeUntilExpiry,
            period: Timeout.InfiniteTimeSpan
        );
    }

    public void Cancel()
    {
        preExpiryTimer?.Dispose();
        preExpiryTimer = null;

        expiryTimer?.Dispose();
        expiryTimer = null;
    }
}
