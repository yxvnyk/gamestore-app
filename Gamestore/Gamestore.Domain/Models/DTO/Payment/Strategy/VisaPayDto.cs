namespace Gamestore.Domain.Models.DTO.Payment.Strategy;

public class VisaPayDto
{
    public string Holder { get; set; }

    public string CardNumber { get; set; }

    public int MonthExpire { get; set; }

    public int YearExpire { get; set; }

    public int Cvv2 { get; set; }
}
