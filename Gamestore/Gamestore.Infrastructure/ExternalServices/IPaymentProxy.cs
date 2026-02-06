using Gamestore.Domain.Models.DTO.Payment.Transaction;

namespace Gamestore.Infrastructure.ExternalServices;

public interface IPaymentProxy
{
    Task<IBoxTransactionResponse> PayIBoxAsync(IBoxTransactionRequest dto);

    Task PayVisaAsync(VisaTransactionRequest dto);
}
