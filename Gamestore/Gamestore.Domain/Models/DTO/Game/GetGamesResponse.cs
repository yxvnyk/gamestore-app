namespace Gamestore.Domain.Models.DTO.Game;

public class GetGamesResponse
{
    public ICollection<GameDto> Games { get; set; }

    public int TotalPages { get; set; }

    public int CurrentPage { get; set; }
}
