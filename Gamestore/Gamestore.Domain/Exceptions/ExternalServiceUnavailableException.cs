namespace Gamestore.Domain.Exceptions;

public class ExternalServiceUnavailableException(string message = "Payment service temporarely unavailable", string title = "Unavailable service") :
    BaseCustomlException(message, 500, title);