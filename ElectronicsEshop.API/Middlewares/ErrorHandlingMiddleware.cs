using ElectronicsEshop.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace ElectronicsEshop.API.Middlewares;

public sealed class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            ProblemDetails problem;

            if(ex is DbUpdateException dbEx && IsForeignKeyConflict(dbEx))
            {
                logger.LogWarning("Conflict (FK) ({TraceId}): {Message}", context.TraceIdentifier, dbEx.Message);
                problem = Create(409, "Conflict", "Entity is referenced and cannot be deleted.", context);
            }
            else switch (ex)
            {
                case NotFoundException:
                    logger.LogWarning("Not Found ({TraceId}): {Message}", context.TraceIdentifier, ex.Message);
                    problem = Create(404, "Not Found", ex.Message, context);
                    break;

                case ConflictException:
                    logger.LogWarning("Conflict ({TraceId}): {Message}", context.TraceIdentifier, ex.Message);
                    problem = Create(409, "Conflict", ex.Message, context);
                    break;

                case ForbiddenException:
                    logger.LogWarning("Forbidden ({TraceId}): {Message}", context.TraceIdentifier, ex.Message);
                    problem = Create(403, "Forbidden", ex.Message, context);
                    break;

                case UnauthorizedException:
                    logger.LogWarning("Unauthorized ({TraceId}): {Message}", context.TraceIdentifier, ex.Message);
                    problem = Create(401, "Unauthorized", ex.Message, context);
                    break;

                case DomainException:
                    logger.LogWarning("Domain Error ({TraceId}): {Message}", context.TraceIdentifier, ex.Message);
                    problem = Create(409, "Domain Error", ex.Message, context);
                    break;

                default:
                    logger.LogError(ex, "Unhandled exception ({TraceId})", context.TraceIdentifier);
                    problem = Create(500, "Server Error", "An unexpected error occurred.", context);
                    break;
            }

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

    private static bool IsForeignKeyConflict(DbUpdateException ex)
    {
        if (ex.InnerException is SqlException sql && sql.Number == 547)
            return true;

        return false;
    }
}
