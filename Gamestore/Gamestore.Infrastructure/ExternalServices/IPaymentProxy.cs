using Gamestore.Domain.Models.DTO.Payment.Provider;

namespace Gamestore.Infrastructure.ExternalServices;

public interface IPaymentProxy
{
    Task<IBoxTransactionResponse> PayIBoxAsync(IBoxTransactionRequest dto);

    Task PayVisaAsync(VisaTransactionRequestDto dto);
}
