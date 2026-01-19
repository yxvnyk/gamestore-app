namespace Gamestore.Domain.Exceptions;

public class NotFoundException(string message = "Resource not found", string title = "Not found") :
    ApiBaseException(message, 404, title);