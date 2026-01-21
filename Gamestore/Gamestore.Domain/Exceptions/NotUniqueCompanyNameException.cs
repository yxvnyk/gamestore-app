namespace Gamestore.Domain.Exceptions;

public class NotUniqueCompanyNameException(string message = "Company name isn't unique", string title = "Not unique") :
    BaseException(message, 409, title);