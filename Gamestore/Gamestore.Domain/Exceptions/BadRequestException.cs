namespace Gamestore.Domain.Exceptions;

public class BadRequestException(string message = "Bad request", string title = "Bad request") :
    BaseCustomlException(message, 400, title);