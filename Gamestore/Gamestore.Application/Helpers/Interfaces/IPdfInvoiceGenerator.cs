using Gamestore.Domain.Models.DTO.Payment;

namespace Gamestore.Application.Helpers.Interfaces;

public interface IPdfInvoiceGenerator
{
    byte[] GenerateInvoice(InvoiceDataDto data);
}
