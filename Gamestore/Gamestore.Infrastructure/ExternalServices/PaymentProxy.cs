using System.Net;
using System.Net.Http.Json;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models.DTO.Payment.Transaction;

namespace Gamestore.Infrastructure.ExternalServices;

public class PaymentProxy(HttpClient httpClient) : IPaymentProxy
{
    private const string IBoxApiPath = "/api/payments/ibox";
    private const string VisaApiPath = "/api/payments/visa";

    public async Task<BoxTransactionResponse> PayIBoxAsync(BoxTransactionRequest dto)
    {
        var response = await httpClient.PostAsJsonAsync(IBoxApiPath, dto);

        EnsureSuccessAsync(response);

        var result = await response.Content.ReadFromJsonAsync<BoxTransactionResponse>();
        return result ?? throw new ExternalServiceUnavailableException("Empty response from payment provider");
    }

    public async Task PayVisaAsync(VisaTransactionRequest dto)
    {
        var response = await httpClient.PostAsJsonAsync(VisaApiPath, dto);
        EnsureSuccessAsync(response);
    }

    // Единая точка обработки ошибок для всех методов оплаты
    private static void EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        throw response.StatusCode switch
        {
            HttpStatusCode.PaymentRequired => new PaymentDeclinedException("Insufficient funds or limit reached"),
            HttpStatusCode.BadRequest => new BadRequestException("Invalid payment data sent to provider"),
            HttpStatusCode.Continue => throw new NotImplementedException(),
            HttpStatusCode.SwitchingProtocols => throw new NotImplementedException(),
            HttpStatusCode.Processing => throw new NotImplementedException(),
            HttpStatusCode.EarlyHints => throw new NotImplementedException(),
            HttpStatusCode.OK => throw new NotImplementedException(),
            HttpStatusCode.Created => throw new NotImplementedException(),
            HttpStatusCode.Accepted => throw new NotImplementedException(),
            HttpStatusCode.NonAuthoritativeInformation => throw new NotImplementedException(),
            HttpStatusCode.NoContent => throw new NotImplementedException(),
            HttpStatusCode.ResetContent => throw new NotImplementedException(),
            HttpStatusCode.PartialContent => throw new NotImplementedException(),
            HttpStatusCode.MultiStatus => throw new NotImplementedException(),
            HttpStatusCode.AlreadyReported => throw new NotImplementedException(),
            HttpStatusCode.IMUsed => throw new NotImplementedException(),
            HttpStatusCode.Ambiguous => throw new NotImplementedException(),
            HttpStatusCode.Moved => throw new NotImplementedException(),
            HttpStatusCode.Found => throw new NotImplementedException(),
            HttpStatusCode.RedirectMethod => throw new NotImplementedException(),
            HttpStatusCode.NotModified => throw new NotImplementedException(),
            HttpStatusCode.UseProxy => throw new NotImplementedException(),
            HttpStatusCode.Unused => throw new NotImplementedException(),
            HttpStatusCode.RedirectKeepVerb => throw new NotImplementedException(),
            HttpStatusCode.PermanentRedirect => throw new NotImplementedException(),
            HttpStatusCode.Unauthorized => throw new NotImplementedException(),
            HttpStatusCode.Forbidden => throw new NotImplementedException(),
            HttpStatusCode.NotFound => throw new NotImplementedException(),
            HttpStatusCode.MethodNotAllowed => throw new NotImplementedException(),
            HttpStatusCode.NotAcceptable => throw new NotImplementedException(),
            HttpStatusCode.ProxyAuthenticationRequired => throw new NotImplementedException(),
            HttpStatusCode.RequestTimeout => throw new NotImplementedException(),
            HttpStatusCode.Conflict => throw new NotImplementedException(),
            HttpStatusCode.Gone => throw new NotImplementedException(),
            HttpStatusCode.LengthRequired => throw new NotImplementedException(),
            HttpStatusCode.PreconditionFailed => throw new NotImplementedException(),
            HttpStatusCode.RequestEntityTooLarge => throw new NotImplementedException(),
            HttpStatusCode.RequestUriTooLong => throw new NotImplementedException(),
            HttpStatusCode.UnsupportedMediaType => throw new NotImplementedException(),
            HttpStatusCode.RequestedRangeNotSatisfiable => throw new NotImplementedException(),
            HttpStatusCode.ExpectationFailed => throw new NotImplementedException(),
            HttpStatusCode.MisdirectedRequest => throw new NotImplementedException(),
            HttpStatusCode.UnprocessableEntity => throw new NotImplementedException(),
            HttpStatusCode.Locked => throw new NotImplementedException(),
            HttpStatusCode.FailedDependency => throw new NotImplementedException(),
            HttpStatusCode.UpgradeRequired => throw new NotImplementedException(),
            HttpStatusCode.PreconditionRequired => throw new NotImplementedException(),
            HttpStatusCode.TooManyRequests => throw new NotImplementedException(),
            HttpStatusCode.RequestHeaderFieldsTooLarge => throw new NotImplementedException(),
            HttpStatusCode.UnavailableForLegalReasons => throw new NotImplementedException(),
            HttpStatusCode.InternalServerError => throw new NotImplementedException(),
            HttpStatusCode.NotImplemented => throw new NotImplementedException(),
            HttpStatusCode.BadGateway => throw new NotImplementedException(),
            HttpStatusCode.ServiceUnavailable => throw new NotImplementedException(),
            HttpStatusCode.GatewayTimeout => throw new NotImplementedException(),
            HttpStatusCode.HttpVersionNotSupported => throw new NotImplementedException(),
            HttpStatusCode.VariantAlsoNegotiates => throw new NotImplementedException(),
            HttpStatusCode.InsufficientStorage => throw new NotImplementedException(),
            HttpStatusCode.LoopDetected => throw new NotImplementedException(),
            HttpStatusCode.NotExtended => throw new NotImplementedException(),
            HttpStatusCode.NetworkAuthenticationRequired => throw new NotImplementedException(),
            _ => new ExternalServiceUnavailableException($"Provider returned {response.StatusCode}"),
        };
    }
}