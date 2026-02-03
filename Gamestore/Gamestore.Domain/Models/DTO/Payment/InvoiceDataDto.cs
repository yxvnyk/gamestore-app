namespace Gamestore.Domain.Models.DTO.Payment;

public record InvoiceDataDto(
    Guid OrderId,
    Guid UserId,
    double Amount,
    DateTime CreatedAt,
    DateTime? ValidUntil);