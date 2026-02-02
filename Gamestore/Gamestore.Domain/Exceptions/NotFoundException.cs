namespace Gamestore.Domain.Exceptions;

public class NotFoundException(string message = "Resource not found", string title = "Not found") :
    BaseInternalException(message, 404, title);