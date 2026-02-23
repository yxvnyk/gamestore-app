using System.Net;
using System.Text;
using Gamestore.WebApi.Helpers.Factories;
using Microsoft.AspNetCore.Http;

namespace Gamestore.WebApi.Tests.Helpers.Factories;

public class RequestDetailsFactoryTests
{
    [Fact]
    public async Task CreateAsync_ReturnsCorrectModel_ForNormalRequestAndResponse()
    {
        // Arrange
        var requestContent = "request body";
        var responseContent = "response body";
        var context = CreateHttpContext(
            requestContent: requestContent,
            responseContent: responseContent,
            remoteIp: "192.168.1.1",
            path: "/api/test",
            statusCode: 201,
            elapsedTime: "123ms");

        // Act
        var result = await RequestDetailsFactory.CreateAsync(context);

        // Assert
        Assert.Equal("192.168.1.1", result.RemoteIpAddress);
        Assert.Equal("/api/test", result.TargetUrl);
        Assert.Equal(201, result.ResponseStatusCode);
        Assert.Equal(requestContent, result.RequestContent);
        Assert.Equal(responseContent, result.ResponseContent);
        Assert.Equal("123ms", result.ElapsedTime);
    }

    [Fact]
    public async Task CreateAsync_ReturnsEmptyStrings_ForEmptyStreams()
    {
        // Arrange
        var context = CreateHttpContext(requestContent: string.Empty, responseContent: string.Empty);

        // Act
        var result = await RequestDetailsFactory.CreateAsync(context);

        // Assert
        Assert.Equal(string.Empty, result.RequestContent);
        Assert.Equal(string.Empty, result.ResponseContent);
    }

    [Fact]
    public async Task CreateAsync_ReturnsMessage_WhenRequestBodyTooLarge()
    {
        // Arrange
        var largeContent = new string('a', 3000);
        var context = CreateHttpContext(requestContent: largeContent, responseContent: "ok");

        // Act
        var result = await RequestDetailsFactory.CreateAsync(context);

        // Assert
        Assert.StartsWith("Request body too large", result.RequestContent);
        Assert.Equal("ok", result.ResponseContent);
    }

    [Fact]
    public async Task CreateAsync_ReturnsNA_WhenNoIpOrElapsedTime()
    {
        // Arrange
        var context = CreateHttpContext(requestContent: "req", responseContent: "resp");
        context.Connection.RemoteIpAddress = null;
        context.Items.Remove("ElapsedTime");

        // Act
        var result = await RequestDetailsFactory.CreateAsync(context);

        // Assert
        Assert.Equal("N/A", result.RemoteIpAddress);
        Assert.Equal("N/A", result.ElapsedTime);
    }

    private static DefaultHttpContext CreateHttpContext(
        string requestContent = "",
        string responseContent = "",
        string? remoteIp = "127.0.0.1",
        string path = "/",
        int statusCode = 200,
        string elapsedTime = "100ms")
    {
        var context = new DefaultHttpContext();

        // Request
        var requestBytes = Encoding.UTF8.GetBytes(requestContent);
        context.Request.Body = new MemoryStream(requestBytes);
        context.Request.Path = path;

        // Response
        var responseBytes = Encoding.UTF8.GetBytes(responseContent);
        context.Response.Body = new MemoryStream(responseBytes);
        context.Response.StatusCode = statusCode;

        // Connection
        if (remoteIp != null)
        {
            context.Connection.RemoteIpAddress = IPAddress.Parse(remoteIp);
        }

        // Items
        context.Items["ElapsedTime"] = elapsedTime;

        return context;
    }
}