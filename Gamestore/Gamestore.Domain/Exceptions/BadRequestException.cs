namespace Gamestore.Domain.Exceptions;

public class BadRequestException(string message = "Bad request", string title = "Bad request") :
    BaseInternalException(message, 400, title);