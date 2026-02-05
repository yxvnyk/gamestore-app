namespace Gamestore.Domain.Models.DTO.Payment.Provider;

public class IBoxTransactionRequest
{
    public double TransactionAmount { get; set; }

    public Guid AccountNumber { get; set; }

    public Guid InvoiceNumber { get; set; }
}
