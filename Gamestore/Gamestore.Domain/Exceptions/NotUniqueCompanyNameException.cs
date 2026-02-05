namespace Gamestore.Domain.Exceptions;

public class NotUniqueCompanyNameException(string message = "Company name isn't unique", string title = "Not unique") :
    BaseCustomlException(message, 409, title);