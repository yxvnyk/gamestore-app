using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.WebApi.Helpers.Middleware;

public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        if (ex is AggregateException aggregateException)
        {
            ex = aggregateException.InnerException ?? ex;
        }

        logger.LogException(ex);
        switch (ex)
        {
            case BaseCustomlException apiEx:
                await HandCustomExceptionAsync(context, apiEx);
                break;
            case DbUpdateException dbEx when IsUniqueKeyViolation(dbEx, "IX_Games_Key"):
                await HandlDBUpdatelException(context, "A game with the same key already exists.");
                break;
            case DbUpdateException dbUpdateEx when IsForeignKeyViolation(dbUpdateEx, "FK_Games_Publishers_PublisherId"):
                await HandlDBUpdatelException(context, "Cannot insert game because the publisher does not exist.");
                break;
            case DbUpdateException dbUpdateEx when IsForeignKeyViolation(dbUpdateEx, "FK_GameGenres_Games_GameId"):
                await HandlDBUpdatelException(context, "Cannot insert game - genre relation because the game does not exist.");
                break;
            case DbUpdateException dbUpdateEx when IsUniqueKeyViolation(dbUpdateEx, "IX_Platforms_Type"):
                await HandlDBUpdatelException(context, "A platform with the same type already exists.");
                break;
            case DbUpdateException dbUpdateEx when IsUniqueKeyViolation(dbUpdateEx, "IX_Genres_Name"):
                await HandlDBUpdatelException(context, "A genre with the same name already exists.");
                break;
            case DbUpdateException dbUpdateEx when IsForeignKeyViolation(dbUpdateEx, "FK_Genres_Genres_ParentGenreId"):
                await HandlDBUpdatelException(context, "Cannot delete genre because it has child genres.");
                break;
            case TimeoutException:
                await HandleTimeoutException(context);
                break;
            case SqlException:
                await HandleSqlException(context);
                break;
            default:
                await UnknownExceptionHandler(context);
                break;
        }
    }

    private static Task HandCustomExceptionAsync(HttpContext context, BaseCustomlException exception)
    {
        var details = new ProblemDetails()
        {
            Detail = exception.Message,
            Title = exception.ErrorCode,
            Status = exception.StatusCode,
        };

        context.Response.StatusCode = exception.StatusCode;
        return context.Response.WriteAsJsonAsync(details);
    }

    private static async Task HandleTimeoutException(HttpContext context)
    {
        var details = new ProblemDetails()
        {
            Detail = "The request time out.",
            Title = "Timeout error",
            Status = 504,
        };

        context.Response.StatusCode = 504;
        await context.Response.WriteAsJsonAsync(details);
    }

    private static async Task HandlDBUpdatelException(HttpContext context, string text)
    {
        var details = new ProblemDetails()
        {
            Detail = text,
            Title = "Incorrect input data",
            Status = 409,
        };

        context.Response.StatusCode = 409;
        await context.Response.WriteAsJsonAsync(details);
    }

    private static async Task HandleSqlException(HttpContext context)
    {
        var details = new ProblemDetails()
        {
            Detail = "Database error has been occured",
            Title = "Database error",
            Status = 500,
        };

        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(details);
    }

    private static async Task UnknownExceptionHandler(HttpContext context)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Title = "Internal Server Error",
            Detail = "An unexpected error occurred.",
            Status = 500,
        });
    }

    private static bool IsUniqueKeyViolation(DbUpdateException ex, string indexName)
    {
        return ex.InnerException is SqlException sqlEx && sqlEx.Errors.Cast<SqlError>().Any(error =>
                (error.Number == 2627 || error.Number == 2601) && error.Message.Contains(indexName));
    }

    private static bool IsForeignKeyViolation(DbUpdateException ex, string fkName)
    {
        return ex.InnerException is SqlException sqlEx && sqlEx.Errors.Cast<SqlError>().Any(error =>
                error.Number == 547 && error.Message.Contains(fkName));
    }
}
