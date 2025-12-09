namespace Gamestore.Application.Exceptions;

public class ApiBaseException : Exception
{
    protected ApiBaseException(string message, int statusCode, string? errorCode = null)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode!;
    }

    public int StatusCode { get; }

    public string ErrorCode { get; }
}
