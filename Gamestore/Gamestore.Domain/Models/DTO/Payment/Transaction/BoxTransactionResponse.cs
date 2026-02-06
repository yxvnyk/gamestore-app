namespace Gamestore.Domain.Models.DTO.Payment.Transaction;

public class BoxTransactionResponse
{
    public double TransactionAmount { get; set; }

    public Guid AccountNumber { get; set; }

    public Guid InvoiceNumber { get; set; }

    public int PaymentMethod { get; set; }

    public Guid AccountId { get; set; }

    public double Amount { get; set; }
}
