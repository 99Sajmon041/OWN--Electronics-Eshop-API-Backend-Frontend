using System.IdentityModel.Tokens.Jwt;

namespace ElectronicsEshop.Blazor.Services.Auth;

public sealed class TokenExpiryService
{
    private Timer? expiryTimer;

    public void Schedule(JwtSecurityToken jwtToken, Func<Task> onExpiredAsync)
    {
        Cancel();

        var expValue = jwtToken.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)
            ?.Value;

        if (string.IsNullOrWhiteSpace(expValue))
            return;

        if (!long.TryParse(expValue, out var expUnixSeconds))
            return;

        var expiresAtUtc = DateTimeOffset.FromUnixTimeSeconds(expUnixSeconds);
        var timeUntilExpiry = expiresAtUtc - DateTimeOffset.UtcNow;

        if (timeUntilExpiry <= TimeSpan.Zero)
        {
            _ = SafeInvokeAsync(onExpiredAsync);
            return;
        }

        expiryTimer = new Timer(
            _ => _ = SafeInvokeAsync(onExpiredAsync),
            state: null,
            dueTime: timeUntilExpiry,
            period: Timeout.InfiniteTimeSpan
        );
    }

    public void Cancel()
    {
        expiryTimer?.Dispose();
        expiryTimer = null;
    }

    private static async Task SafeInvokeAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
        }
    }
}
