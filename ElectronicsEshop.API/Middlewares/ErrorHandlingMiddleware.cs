using ElectronicsEshop.Application.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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

            if (ex is DbUpdateException dbEx && IsForeignKeyConflict(dbEx))
            {
                logger.LogWarning("Konflikt (FK) ({TraceId}): {Message}", context.TraceIdentifier, dbEx.Message);
                problem = Create(
                    StatusCodes.Status409Conflict,
                    "Konflikt dat",
                    "Entita je referencována v jiné tabulce a nelze ji odstranit.",
                    context);
            }
            else
            {
                switch (ex)
                {
                    case ValidationException vex:
                        {
                            var errorMessages = vex.Errors
                                .Select(e => e.ErrorMessage)
                                .Where(m => !string.IsNullOrWhiteSpace(m))
                                .Distinct()
                                .ToArray();

                            var detail = errorMessages.Length > 0
                                ? string.Join(" ", errorMessages)
                                : "Neplatný vstup.";

                            logger.LogWarning("Neplatný vstup ({TraceId}): {Message}", context.TraceIdentifier, detail);

                            problem = Create(
                                StatusCodes.Status400BadRequest,
                                "Neplatný vstup",
                                detail,
                                context);

                            break;
                        }

                    case NotFoundException:
                        logger.LogWarning("Nenalezeno ({TraceId}): {Message}", context.TraceIdentifier, ex.Message);
                        problem = Create(
                            StatusCodes.Status404NotFound,
                            "Nenalezeno",
                            ex.Message,
                            context);
                        break;

                    case ConflictException:
                        logger.LogWarning("Konflikt ({TraceId}): {Message}", context.TraceIdentifier, ex.Message);
                        problem = Create(
                            StatusCodes.Status409Conflict,
                            "Konflikt",
                            ex.Message,
                            context);
                        break;

                    case ForbiddenException:
                        logger.LogWarning("Zakázáno ({TraceId}): {Message}", context.TraceIdentifier, ex.Message);
                        problem = Create(
                            StatusCodes.Status403Forbidden,
                            "Zakázáno",
                            ex.Message,
                            context);
                        break;

                    case UnauthorizedException:
                        logger.LogWarning("Neautorizováno ({TraceId}): {Message}", context.TraceIdentifier, ex.Message);
                        problem = Create(
                            StatusCodes.Status401Unauthorized,
                            "Neautorizováno",
                            ex.Message,
                            context);
                        break;

                    case DomainException:
                        logger.LogWarning("Chyba domény ({TraceId}): {Message}", context.TraceIdentifier, ex.Message);
                        problem = Create(
                            StatusCodes.Status409Conflict,
                            "Chyba domény",
                            ex.Message,
                            context);
                        break;

                    case ArgumentException:
                        logger.LogWarning("Neplatný argument ({TraceId}): {Message}", context.TraceIdentifier, ex.Message);
                        problem = Create(
                            StatusCodes.Status400BadRequest,
                            "Neplatný vstup",
                            ex.Message,
                            context);
                        break;

                    default:
                        logger.LogError(ex, "Nezpracovaná výjimka ({TraceId})", context.TraceIdentifier);
                        problem = Create(
                            StatusCodes.Status500InternalServerError,
                            "Chyba serveru",
                            "Došlo k neočekávané chybě na serveru.",
                            context);
                        break;
                }
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
        if (errors is not null)
        {
            problem.Extensions["errors"] = errors;
        }

        return problem;
    }

    private static bool IsForeignKeyConflict(DbUpdateException ex)
    {
        if (ex.InnerException is SqlException sql && sql.Number == 547)
            return true;

        return false;
    }
}
