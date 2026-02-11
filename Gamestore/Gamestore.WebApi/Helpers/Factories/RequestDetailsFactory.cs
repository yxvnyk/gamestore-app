using System.Text;
using Gamestore.Domain.Models;

namespace Gamestore.WebApi.Helpers.Factories;

public static class RequestDetailsFactory
{
    private const int MaxContentSize = 2048; // 2 KB

    public static async Task<RequestDetailsModel> CreateAsync(HttpContext context)
    {
        var requestContent = await GetContentFromStreamAsync(context.Request.Body);

        var responseContent = string.Empty;
        if (context.Response.Body.CanSeek)
        {
            responseContent = await GetContentFromStreamAsync(context.Response.Body);
        }

        return new RequestDetailsModel
        {
            RemoteIpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "N/A",
            TargetUrl = context.Request.Path,
            ResponseStatusCode = context.Response.StatusCode,
            RequestContent = requestContent,
            ResponseContent = responseContent,
            ElapsedTime = context.Items["ElapsedTime"]?.ToString() ?? "N/A",
        };
    }

    private static async Task<string> GetContentFromStreamAsync(Stream stream)
    {
        if (stream == null || !stream.CanRead)
        {
            return string.Empty;
        }

        if (stream.Length > MaxContentSize)
        {
            return $"Request body too large ({stream.Length} bytes), skipping log";
        }

        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
        string body = await reader.ReadToEndAsync();

        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        return body;
    }
}
