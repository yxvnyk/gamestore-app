using System.Net;
using Gamestore.WebApi.Helpers.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Gamestore.WebApi.Tests.Helpers.Middleware;

public class RequestDetailsLoggingMiddlewareTests
{
    private const string RequestContent = "request";
    private const string RemoteIpAddress = "127.0.0.1";
    private const string TargetUrl = "/test";
    private const int ResponseStatusCode = StatusCodes.Status200OK;
    private const string ElapsedTime = "100ms";
    private const string ResponseContent = "response";

    [Fact]
    public async Task InvokeAsync_LogsRequestDetails()
    {
        // Arrange
        var context = CreateHttpContextWithRequestDetails();
        var loggerMock = new Mock<ILogger<RequestDetailsLoggingMiddleware>>();
        var middleware = new RequestDetailsLoggingMiddleware(loggerMock.Object);
        loggerMock.Setup(l => l.IsEnabled(LogLevel.Information)).Returns(true);

        // Act
        await middleware.InvokeAsync(context, async ctx =>
        {
            var responseBytes = System.Text.Encoding.UTF8.GetBytes(ResponseContent);
            await ctx.Response.Body.WriteAsync(responseBytes);
        });

        // Assert
        var invocation = loggerMock.Invocations
            .FirstOrDefault(i => i.Method.Name == "Log" && i.Arguments.Count > 0 && i.Arguments[0] is LogLevel lvl && lvl == LogLevel.Information);

        Assert.NotNull(invocation);
        var state = invocation.Arguments[2];
        var formatted = state?.ToString() ?? string.Empty;
        Assert.Contains(RequestContent, formatted);
        Assert.Contains(RemoteIpAddress, formatted);
        Assert.Contains(TargetUrl, formatted);
        Assert.Contains(ResponseStatusCode.ToString(), formatted);
        Assert.Contains(ElapsedTime, formatted);
        Assert.Contains(ResponseContent, formatted);
    }

    [Fact]
    public async Task InvokeAsync_ResponseBodyIsRestored()
    {
        // Arrange
        var context = CreateHttpContextWithRequestDetails();
        var loggerMock = new Mock<ILogger<RequestDetailsLoggingMiddleware>>();
        var middleware = new RequestDetailsLoggingMiddleware(loggerMock.Object);
        loggerMock.Setup(l => l.IsEnabled(LogLevel.Information)).Returns(true);

        // Act
        await middleware.InvokeAsync(context, async ctx =>
        {
            var responseBytes = System.Text.Encoding.UTF8.GetBytes(ResponseContent);
            await ctx.Response.Body.WriteAsync(responseBytes);
        });

        // Assert: тело ответа возвращено в исходный поток
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        Assert.Contains(ResponseContent, body);
    }

    private static DefaultHttpContext CreateHttpContextWithRequestDetails()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddControllers();
        context.RequestServices = services.BuildServiceProvider();

        var requestBytes = System.Text.Encoding.UTF8.GetBytes(RequestContent);
        context.Request.Body = new MemoryStream(requestBytes);

        context.Connection.RemoteIpAddress = IPAddress.Parse(RemoteIpAddress);
        context.Request.Path = TargetUrl;
        context.Response.StatusCode = ResponseStatusCode;
        context.Items["ElapsedTime"] = ElapsedTime;

        return context;
    }
}