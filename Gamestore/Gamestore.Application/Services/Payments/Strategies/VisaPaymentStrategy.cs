using Gamestore.Application.Services.Interfaces.Payments;
using Gamestore.Domain.Models.DTO.Payment;
using Gamestore.Domain.Models.DTO.Payment.Strategy;
using Gamestore.Domain.Models.DTO.Payment.Transaction;
using Gamestore.Infrastructure.ExternalServices;

namespace Gamestore.Application.Services.Payments.Strategies;

public class VisaPaymentStrategy(IPaymentProxy paymentProxy) : IPaymentStrategy
{
    private const string PaymentMethod = "Visa";

    public string PaymentMethodName()
    {
        return PaymentMethod;
    }

    public async Task<PaymentResult> ProcessPaymentAsync(PaymentContextDto context)
    {
        if (context.VisaModel is not VisaPayDto visaData)
        {
            throw new InvalidOperationException("VisaPaymentStrategy wait for VisaPaymentDetails");
        }

        var transactionDto = new VisaTransactionRequest
        {
            CardNumber = visaData.CardNumber,
            CardHolderName = visaData.CardNumber,
            ExpirationMonth = visaData.MonthExpire,
            Cvv = visaData.Cvv2,
            TransactionAmount = (decimal)context.Amount,
            ExpirationYear = visaData.YearExpire,
        };

        await paymentProxy.PayVisaAsync(transactionDto);

        return PaymentResult.Success();
    }
}
