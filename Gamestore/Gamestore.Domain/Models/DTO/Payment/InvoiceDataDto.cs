namespace Gamestore.Domain.Models.DTO.Payment;

public record InvoiceDataDto(
    Guid OrderId,
    Guid CustomerId,
    double Amount,
    DateTime CreatedAt,
    DateTime ValidUntil);