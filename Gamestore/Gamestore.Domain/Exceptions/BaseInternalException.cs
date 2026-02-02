namespace Gamestore.Domain.Exceptions;

public class BaseInternalException : Exception
{
    protected BaseInternalException(string message, int statusCode, string? errorCode = null)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode!;
    }

    public int StatusCode { get; }

    public string ErrorCode { get; }
}
