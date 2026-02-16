namespace Gamestore.Domain.Exceptions;

public class PaymentDeclinedException(string message = "Unsufficient funds or transaction declined by the bank", string title = "Payment declined") :
    BaseCustomlException(message, 409, title);