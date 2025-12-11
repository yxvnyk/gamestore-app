using Gamestore.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.WebApi.Helpers.Middleware;

public class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ApiBaseException ex)
        {
            await HandleApiExceptionAsync(context, ex);
        }
        catch (DbUpdateException ex) when (IsUniqueKeyViolation(ex, "IX_Games_Key"))
        {
            await HandlDBUpdatelException(context, "A game with the same key already exists.");
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex, "FK_GameGenres_Games_GameId"))
        {
            await HandlDBUpdatelException(context, "Cannot insert game - genre relation because the game does not exist.");
        }
        catch (DbUpdateException ex) when (IsUniqueKeyViolation(ex, "IX_Platforms_Type"))
        {
            await HandlDBUpdatelException(context, "A platform with the same type already exists.");
        }
        catch (DbUpdateException ex) when (IsUniqueKeyViolation(ex, "IX_Genres_Name"))
        {
            await HandlDBUpdatelException(context, "A genre with the same name already exists.");
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex, "FK_Genres_Genres_ParentGenreId"))
        {
            await HandlDBUpdatelException(context, "Cannot delete genre because it has child genres.");
        }
        catch (TimeoutException)
        {
            await HandleTimeoutException(context);
        }
        catch (SqlException)
        {
            await HandleSqlException(context);
        }
    }

    private static Task HandleApiExceptionAsync(HttpContext context, ApiBaseException exception)
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
