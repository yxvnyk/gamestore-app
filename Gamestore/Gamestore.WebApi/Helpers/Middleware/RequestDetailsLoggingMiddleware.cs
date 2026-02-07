using Gamestore.Domain.Extensions;
using Gamestore.WebApi.Helpers.Factories;

namespace Gamestore.WebApi.Helpers.Middleware;

public class RequestDetailsLoggingMiddleware(ILogger<RequestDetailsLoggingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Request.EnableBuffering();
        var originalBodyStream = context.Response.Body;
        var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await next(context);

            var requestDetails = await RequestDetailsFactory.CreateAsync(context);
            logger.LogRequestDetails(requestDetails);

            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }
}