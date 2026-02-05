using Gamestore.Domain.Models.DTO.Payment.Provider;

namespace Gamestore.Infrastructure.ExternalServices;

public interface IPaymentProxy
{
    Task<IBoxPayResponseDto> PayIBoxAsync(IBoxPayRequestDto dto);

    Task<IBoxPayResponseDto> PayVisaAsync(IBoxPayRequestDto dto);
}
