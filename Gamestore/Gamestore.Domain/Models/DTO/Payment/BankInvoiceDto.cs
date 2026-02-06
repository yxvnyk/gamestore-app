namespace Gamestore.Domain.Models.DTO.Payment;

public record BankInvoiceDto(
    Guid OrderId,
    Guid UserId,
    double Amount,
    DateTime CreatedAt,
    DateTime? ValidUntil);