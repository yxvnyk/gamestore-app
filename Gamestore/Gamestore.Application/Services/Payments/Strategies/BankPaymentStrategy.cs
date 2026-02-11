using Gamestore.Application.Helpers.Interfaces;
using Gamestore.Application.Services.Interfaces.Payments;
using Gamestore.Domain.Models.Configuration;
using Gamestore.Domain.Models.DTO.Payment;
using Gamestore.Domain.Models.DTO.Payment.Strategy;
using Microsoft.Extensions.Options;

namespace Gamestore.Application.Services.Payments.Strategies;

public class BankPaymentStrategy(IPdfInvoiceGenerator pdfGenerator, IOptions<PaymentOptions> paymentOptions) : IPaymentStrategy
{
    private const string PaymentMethod = "Bank";

    private readonly int _validityDays = paymentOptions.Value.InvoiceValidityDays;

    public string PaymentMethodName()
    {
        return PaymentMethod;
    }

    public async Task<PaymentResult> ProcessPaymentAsync(PaymentContextDto context)
    {
        var creationDate = DateTime.UtcNow;
        var validUntil = creationDate.AddDays(_validityDays);

        var invoiceData = new BankInvoiceDto(
            OrderId: context.OrderId,
            UserId: context.CustomerId,
            Amount: context.Amount,
            CreatedAt: creationDate,
            ValidUntil: validUntil);

        byte[] fileBytes = pdfGenerator.GenerateInvoice(invoiceData);

        return await Task.FromResult(PaymentResult.SuccessWithFile(fileBytes, $"Invoice_{context.OrderId}.pdf"));
    }
}
