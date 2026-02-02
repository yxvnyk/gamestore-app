using Gamestore.Application.Helpers.Interfaces;
using Gamestore.Domain.Models.Configuration;
using Gamestore.Domain.Models.DTO.Payment;
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

    public async Task<PaymentResult> ProcessPaymentAsync(Guid customerId, Guid orderId, double amount)
    {
        var creationDate = DateTime.UtcNow;
        var validUntil = creationDate.AddDays(_validityDays);

        var invoiceData = new InvoiceDataDto(
            OrderId: orderId,
            CustomerId: customerId,
            Amount: amount,
            CreatedAt: creationDate,
            ValidUntil: validUntil);

        byte[] fileBytes = pdfGenerator.GenerateInvoice(invoiceData);

        return await Task.FromResult(PaymentResult.SuccessWithFile(fileBytes, $"Invoice_{orderId}.pdf"));
    }
}
