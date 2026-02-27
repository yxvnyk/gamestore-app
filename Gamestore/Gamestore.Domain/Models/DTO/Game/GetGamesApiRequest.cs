namespace Gamestore.Domain.Models.DTO.Game;

public class GetGamesApiRequest
{
    public double? MinPrice { get; set; }

    public double? MaxPrice { get; set; }

    public string? Name { get; set; }

    public string? DatePublishing { get; set; }

    public string? Sort { get; set; }

    public int Page { get; set; } = 1;

    public string? PageCount { get; set; } = string.Empty;

    public ICollection<Guid>? Genres { get; set; }

    public ICollection<Guid>? Platforms { get; set; }

    public ICollection<Guid>? Publishers { get; set; }

    public string? Trigger { get; set; }
}
