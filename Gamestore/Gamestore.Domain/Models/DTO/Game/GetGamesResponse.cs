namespace Gamestore.Domain.Models.DTO.Game;

public class GetGamesResponse
{
    public IEnumerable<GameDto> Games { get; set; }

    public int TotalPages { get; set; }

    public int CurrentPage { get; set; }
}
