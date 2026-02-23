namespace Gamestore.Domain.Models.DTO.Payment;

public class PaymentResult
{
    public bool IsSuccess { get; set; }

    public string? ErrorMessage { get; set; }

    public byte[]? FileBytes { get; set; }

    public string? ContentType { get; set; }

    public string? FileName { get; set; }

    public object? Data { get; set; }

    public static PaymentResult Success() => new() { IsSuccess = true };

    public static PaymentResult Fail(string message) => new() { IsSuccess = false, ErrorMessage = message };

    public static PaymentResult SuccessWithFile(byte[] bytes, string name)
        => new() { IsSuccess = true, FileBytes = bytes, ContentType = "application/pdf", FileName = name };

    public static PaymentResult SuccessWithData(object data)
        => new() { IsSuccess = true, Data = data };
}
