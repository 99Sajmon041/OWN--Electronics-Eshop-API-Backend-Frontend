using ElectronicsEshop.Application.User;
using Serilog.Context;

namespace ElectronicsEshop.API.Middlewares;

public sealed class UserLogEnricherMiddleware(RequestDelegate next, IUserContext userContext)
{
    public async Task Invoke(HttpContext context)
    {
        var user = userContext.GetCurrentUser();

        using (LogContext.PushProperty("UserId", user?.Id ?? "anon"))
        using (LogContext.PushProperty("UserEmail", user?.Email ?? "annon"))
        {
            await next.Invoke(context);
        }
    }
}
