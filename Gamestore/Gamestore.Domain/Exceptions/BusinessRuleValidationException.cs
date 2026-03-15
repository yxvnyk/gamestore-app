namespace Gamestore.Domain.Exceptions;

public class BusinessRuleValidationException(string message = "Invalid operatational", string title = "Invalid operatational") :
    BaseCustomlException(message, 400, title);