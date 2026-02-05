namespace Gamestore.Domain.Exceptions;

public class PaymentDeclinedExcetion(string message = "Unsufficient funds or transaction declined by the bank", string title = "Payment declined") :
    BaseCustomlException(message, 409, title);