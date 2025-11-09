using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ElectronicsEshop.Application.User;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUser? GetCurrentUser()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if(user?.Identity == null || !user.Identity.IsAuthenticated)
            return null;

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = user.FindFirstValue(ClaimTypes.Email);

        if(string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(email))
            return null;

        var roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();

        DateOnly dateOfBirth = default;
        var dobString = user.FindFirstValue(ClaimTypes.DateOfBirth);
        if(!string.IsNullOrWhiteSpace(dobString) && DateOnly.TryParseExact(dobString, "yyyy-MM-dd", out var dob))
        {
            dateOfBirth = dob;
        }

        return new CurrentUser(userId, email, roles, dateOfBirth);
    }
}
