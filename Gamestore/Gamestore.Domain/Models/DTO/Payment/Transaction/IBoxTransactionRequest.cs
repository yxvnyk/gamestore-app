namespace Gamestore.Domain.Models.DTO.Payment.Transaction;

public class IBoxTransactionRequest
{
    public double TransactionAmount { get; set; }

    public Guid AccountNumber { get; set; }

    public Guid InvoiceNumber { get; set; }
}
