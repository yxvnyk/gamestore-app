using Gamestore.Application.Helpers.Interfaces;
using Gamestore.Domain.Models.DTO.Payment;
using Gamestore.Domain.Models.DTO.Payment.Provider;
using Gamestore.Domain.Models.DTO.Payment.Strategy;
using Gamestore.Infrastructure.ExternalServices;

namespace Gamestore.Application.Helpers;

public class VisaPaymentStrategy(IPaymentProxy paymentProxy) : IPaymentStrategy
{
    private const string PaymentMethod = "Visa";

    public string PaymentMethodName()
    {
        return PaymentMethod;
    }

    public async Task<PaymentResult> ProcessPaymentAsync(SimplePayDto payDto)
    {
        if (payDto.VisaDetails is not VisaPayDto visaData)
        {
            throw new InvalidOperationException("VisaPaymentStrategy wait for VisaPaymentDetails");
        }

        var transactionDto = new VisaTransactionRequestDto
        {
            CardNumber = visaData.CardNumber,
            CardHolderName = visaData.CardNumber,
            ExpirationMonth = visaData.MonthExpire,
            Cvv = visaData.Cvv2,
            TransactionAmount = (decimal)payDto.Amount,
            ExpirationYear = visaData.YearExpire,
        };

        await paymentProxy.PayVisaAsync(transactionDto);

        return await Task.FromResult(PaymentResult.Success());
    }
}
