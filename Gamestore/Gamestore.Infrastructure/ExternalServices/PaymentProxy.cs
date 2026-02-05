using System.Net;
using System.Net.Http.Json;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models.DTO.Payment.Provider;

namespace Gamestore.Infrastructure.ExternalServices;

public class PaymentProxy(HttpClient httpClient) : IPaymentProxy
{
    private readonly string _ibox_api_path = "/api/payments/ibox";
    private readonly string _visa_api_path = "/api/payments/visa";

    public async Task<IBoxTransactionResponse> PayIBoxAsync(IBoxTransactionRequest dto)
    {
        var response = await httpClient.PostAsJsonAsync(_ibox_api_path, dto);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<IBoxTransactionResponse>();
            return result ?? throw new ExternalServiceUnavailableException("Empty response from payment provider");
        }

        if (response.StatusCode == HttpStatusCode.PaymentRequired)
        {
            throw new PaymentDeclinedExcetion();
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            throw new BadRequestException();
        }

        throw new ExternalServiceUnavailableException();
    }

    public async Task PayVisaAsync(VisaTransactionRequestDto dto)
    {
        var response = await httpClient.PostAsJsonAsync(_visa_api_path, dto);
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        if (response.StatusCode == HttpStatusCode.PaymentRequired)
        {
            throw new PaymentDeclinedExcetion();
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            throw new BadRequestException();
        }

        throw new ExternalServiceUnavailableException();
    }
}