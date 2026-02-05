using Gamestore.Application.Helpers.Interfaces;
using Gamestore.Domain.Models.Configuration;
using Gamestore.Domain.Models.DTO.Payment;
using Gamestore.Domain.Models.DTO.Payment.Strategy;
using Microsoft.Extensions.Options;

namespace Gamestore.Application.Helpers;

public class BankPaymentStrategy(IPdfInvoiceGenerator pdfGenerator, IOptions<PaymentOptions> paymentOptions) : IPaymentStrategy
{
    private const string PaymentMethod = "Bank";

    private readonly int _validityDays = paymentOptions.Value.InvoiceValidityDays;

    public string PaymentMethodName()
    {
        return PaymentMethod;
    }

    public async Task<PaymentResult> ProcessPaymentAsync(SimplePayDto payDto)
    {
        var creationDate = DateTime.UtcNow;
        var validUntil = creationDate.AddDays(_validityDays);

        var invoiceData = new InvoiceDataDto(
            OrderId: payDto.OrderId,
            UserId: payDto.CustomerId,
            Amount: payDto.Amount,
            CreatedAt: creationDate,
            ValidUntil: validUntil);

        byte[] fileBytes = pdfGenerator.GenerateInvoice(invoiceData);

        return await Task.FromResult(PaymentResult.SuccessWithFile(fileBytes, $"Invoice_{payDto.OrderId}.pdf"));
    }
}
