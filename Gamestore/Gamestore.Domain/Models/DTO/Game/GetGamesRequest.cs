namespace Gamestore.Domain.Models.DTO.Game;

public class GetGamesRequest
{
    public double? MinPrice { get; set; }

    public double? MaxPrice { get; set; }

    public string? Name { get; set; }

    public string? DatePublishing { get; set; }

    public string? Sort { get; set; }

    public int Page { get; set; } = 1;

    public string? PageCount { get; set; } = string.Empty;

    public int ActualPageSize { get; set; }

    public string? Trigger { get; set; }
}
