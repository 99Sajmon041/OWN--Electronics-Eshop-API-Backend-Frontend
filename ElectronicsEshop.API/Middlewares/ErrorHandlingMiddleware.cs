using ElectronicsEshop.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsEshop.API.Middlewares;

public sealed class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception ({TraceId})", context.TraceIdentifier);

            var problem = ex switch
            {
                ValidationException v => Create(400, "Validation Failed", v.Message, context, v.Errors),
                NotFoundException => Create(404, "Not Found", ex.Message, context),
                ForbiddenException => Create(403, "Forbidden", ex.Message, context),
                UnauthorizedException => Create(401, "Unauthorized", ex.Message, context),
                ConflictException => Create(409, "Conflict", ex.Message, context),
                DomainException => Create(409, "Domain Error", ex.Message, context),
                _ => Create(500, "Server Error", "An unexpected error occurred.", context)
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(problem);
        }
    }

    private static ProblemDetails Create(int status, string title, string detail, HttpContext ctx, object? errors = null)
    {
        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Type = $"https://httpstatuses.io/{status}",
            Instance = ctx.Request.Path
        };
        problem.Extensions["traceId"] = ctx.TraceIdentifier;
        if (errors is not null) problem.Extensions["errors"] = errors;

        return problem;
    }
}
