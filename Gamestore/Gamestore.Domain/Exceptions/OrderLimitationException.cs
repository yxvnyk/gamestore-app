namespace Gamestore.Domain.Exceptions;

public class OrderLimitationException(string message = "User cannot order more games than available in the stock", string title = "Order limitation") :
    BaseException(message, 409, title);