namespace Gamestore.Domain.Models.DTO.Payment.Provider;

public class VisaTransactionRequestDto
{
    public decimal TransactionAmount { get; set; }

    public string CardHolderName { get; set; }

    public string CardNumber { get; set; }

    public int ExpirationMonth { get; set; }

    public int ExpirationYear { get; set; }

    public int Cvv { get; set; }
}
