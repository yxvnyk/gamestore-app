namespace Gamestore.Domain.Models.DTO.Game;

public class CreateGameRequest
{
    public GameDto Game { get; set; }

    public Guid[] Genres { get; set; }

    public Guid[] Platforms { get; set; }

    public Guid Publisher { get; set; }
}
