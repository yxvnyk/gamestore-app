namespace Gamestore.Domain.Models;

public class RequestDetailsModel
{
    public string? RemoteIpAddress { get; set; }

    public string? TargetUrl { get; set; }

    public int ResponseStatusCode { get; set; }

    public string? RequestContent { get; set; }

    public string? ResponseContent { get; set; }

    public string? ElapsedTime { get; set; }
}
