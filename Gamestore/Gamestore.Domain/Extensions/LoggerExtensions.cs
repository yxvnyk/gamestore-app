using System.Collections;
using System.Text;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Gamestore.Domain.Extensions;

/// <summary>
/// Provides extension methods for logging with various log levels.
/// </summary>
public static partial class LoggerExtensions
{
    /// <summary>
    /// Defines a log action for logging request details.
    /// </summary>
    private static readonly Action<ILogger, string, string, int, string, string, string, Exception?> RequestDetails =
    LoggerMessage.Define<string, string, int, string, string, string>(
        LogLevel.Information,
        new EventId(1003, "Request details logging"),
        "User IP Address: {ip},\nUrl: {url},\n Status: {status},\n Request: {request},\n Response: {response},\n Elapsed: {elapsed}");

    /// <summary>
    /// Defines a log action for logging exceptions.
    /// </summary>
    private static readonly Action<ILogger, string, string, string?, string?, string?, Exception?> Exception =
    LoggerMessage.Define<string, string, string?, string?, string?>(
        LogLevel.Error,
        new EventId(1003, "Exception logging"),
        "Exception type: {type}\nExceptions mesage: {message}\nInner exception: {innerException}\nException details: {details}\nStack trace: {trace}");

    /// <summary>
    /// Defines a log action for logging trace-level messages.
    /// </summary>
    private static readonly Action<ILogger, string, Exception?> Trace =
    LoggerMessage.Define<string>(
        LogLevel.Trace,
        new EventId(1003, "Trace logging"),
        "Received request to {Class}");

    /// <summary>
    /// Logs detailed information about an HTTP request and response.
    /// </summary>
    /// <param name="logger">The logger instance used to write the log entry.</param>
    /// <param name="model">The <see cref="RequestDetailsModel"/> containing request and response details to be logged.</param>
    public static void LogRequestDetails(this ILogger logger, RequestDetailsModel model)
    {
        RequestDetails(
            logger,
            model.RemoteIpAddress ?? "N/A",
            model.TargetUrl ?? "N/A",
            model.ResponseStatusCode,
            model.RequestContent ?? "N/A",
            model.ResponseContent ?? "N/A",
            model.ElapsedTime ?? "N/A",
            null);
    }

    /// <summary>
    /// Logs an error message with the specified class name.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="exception">The <see cref="Exception"/> containig details to be logged. </param>
    public static void LogException(this ILogger logger, Exception exception)
    {
        var details = AgregateExceptionDetails(exception);
        var innerExceptions = GetAllInnerExceptions(exception);
        Exception(logger, exception.GetType().Name, exception.Message, innerExceptions, details, exception.StackTrace, exception);
    }

    /// <summary>
    /// Logs a trace message with the specified class name.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="class">The name of the class from which the log is generated.</param>
    public static void LogTrace(this ILogger logger, string @class)
    {
        Trace(logger, @class, null);
    }

    private static string AgregateExceptionDetails(Exception exception)
    {
        var details = new StringBuilder();
        if (exception.Data?.Count > 0)
        {
            details.AppendLine("Exception Data:");
            foreach (DictionaryEntry de in exception.Data)
            {
                details.AppendLine($"  {de.Key}: {de.Value}");
            }
        }

        if (exception is BaseCustomlException apiEx)
        {
            details.AppendLine($" Status code: {apiEx.StatusCode}");
            details.AppendLine($" Error code: {apiEx.ErrorCode}");
        }

        return details.Length > 0 ? details.ToString() : null;
    }

    private static string GetAllInnerExceptions(Exception ex)
    {
        var sb = new StringBuilder();
        var current = ex.InnerException;
        while (current != null)
        {
            sb.AppendLine(current.ToString());
            current = current.InnerException;
        }

        return sb.ToString();
    }
}
