using System.Reflection;
using System.Text.Json;
using Gamestore.Application.Exceptions;
using Gamestore.WebApi.Helpers.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Gamestore.WebApi.Tests.Helpers.Middleware;

public class ExceptionHandlingMiddlewareTests
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    };

    [Fact]
    public async Task InvokeAsyncTryNotFoundException()
    {
        // Arrange
        var context = CreateHttpContext();
        var middleware = new ExceptionHandlingMiddleware();

        // Act
        await middleware.InvokeAsync(context, contx => throw new NotFoundException());

        // Assert
        Assert.True(context.Response.StatusCode == 404);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();

        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(
            body,
            _jsonOptions);

        Assert.NotNull(problemDetails);
        Assert.Equal(404, problemDetails!.Status);
        Assert.Equal("Not found", problemDetails.Title);
        Assert.Equal("Resource not found", problemDetails.Detail);
    }

    [Fact]
    public async Task InvokeAsyncTryTimeOutException()
    {
        // Arrange
        var context = CreateHttpContext();
        var middleware = new ExceptionHandlingMiddleware();

        // Act
        await middleware.InvokeAsync(context, contx => throw new TimeoutException());

        // Assert
        Assert.True(context.Response.StatusCode == 504);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();

        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(
            body,
            _jsonOptions);

        Assert.NotNull(problemDetails);
        Assert.Equal(504, problemDetails!.Status);
        Assert.Equal("Timeout error", problemDetails.Title);
        Assert.Equal("The request time out.", problemDetails.Detail);
    }

    [Fact]
    public async Task InvokeAsyncTrySqlException()
    {
        // Arrange
        var context = CreateHttpContext();
        var middleware = new ExceptionHandlingMiddleware();

        var dummyMessage = "Dummy SQL exception message";
        var dummyNumber = 1234;
        var sqlException = CreateSqlException(dummyNumber, dummyMessage);

        // Act
        await middleware.InvokeAsync(context, contx => throw sqlException);

        // Assert
        Assert.True(context.Response.StatusCode == 500);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();

        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(
            body,
            _jsonOptions);

        Assert.NotNull(problemDetails);
        Assert.Equal(500, problemDetails!.Status);
        Assert.Equal("Database error", problemDetails.Title);
        Assert.Equal("Database error has been occured", problemDetails.Detail);
    }

    [Theory]
    [InlineData("IX_Games_Key", "A game with the same key already exists.", 2627)]
    [InlineData("FK_GameGenres_Games_GameId", "Cannot insert game - genre relation because the game does not exist.", 547)]
    [InlineData("IX_Platforms_Type", "A platform with the same type already exists.", 2627)]
    [InlineData("IX_Genres_Name", "A genre with the same name already exists.", 2627)]
    [InlineData("FK_Genres_Genres_ParentGenreId", "Cannot delete genre because it has child genres.", 547)]
    public async Task InvokeAsyncDbUpdateExceptionReturns409(string constraint, string expectedMesage, int errorNumber)
    {
        // Arrange
        var sqlException = CreateSqlException(errorNumber, constraint);
        var dbEx = new DbUpdateException("error", sqlException);
        var context = CreateHttpContext();
        var middleware = new ExceptionHandlingMiddleware();

        // Act
        await middleware.InvokeAsync(context, contx => throw dbEx);

        // Assert
        Assert.True(context.Response.StatusCode == 409);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();

        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(
            body,
            _jsonOptions);

        Assert.NotNull(problemDetails);
        Assert.Equal(409, problemDetails!.Status);
        Assert.Equal("Incorrect input data", problemDetails.Title);
        Assert.Equal(expectedMesage, problemDetails.Detail);
    }

    private static DefaultHttpContext CreateHttpContext()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddControllers();
        context.RequestServices = services.BuildServiceProvider();
        return context;
    }

    private static SqlException CreateSqlException(int number, string message)
    {
        var ctors = typeof(SqlError).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
        ConstructorInfo sqlErrorCtor = (ctors.FirstOrDefault(c => c.GetParameters().Length == 8)
                                    ?? ctors.FirstOrDefault(c => c.GetParameters().Length == 7)) ?? throw new InvalidOperationException("SqlError constructor not found.");
        var parameters = sqlErrorCtor.GetParameters();
        object sqlError = parameters.Length == 8 && parameters[7].ParameterType == typeof(Exception)
            ? sqlErrorCtor.Invoke([number, (byte)0, (byte)0, string.Empty, message, string.Empty, 1, null])
            : parameters.Length == 7
                ? sqlErrorCtor.Invoke([number, (byte)0, (byte)0, string.Empty, message, string.Empty, 1])
                : throw new InvalidOperationException("Unknown SqlError constructor signature.");
        var sqlErrorCollection = (SqlErrorCollection)Activator.CreateInstance(typeof(SqlErrorCollection), true)!;
        typeof(SqlErrorCollection)
            .GetMethod("Add", BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(sqlErrorCollection, [sqlError]);

        var sqlException = typeof(SqlException)
            .GetMethod("CreateException", BindingFlags.Static | BindingFlags.NonPublic, null, [typeof(SqlErrorCollection), typeof(string)], null)!
            .Invoke(null, [sqlErrorCollection, "7.0.0"]) as SqlException;

        return sqlException!;
    }
}