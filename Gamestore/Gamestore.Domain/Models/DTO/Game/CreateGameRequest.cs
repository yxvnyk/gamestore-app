namespace Gamestore.Domain.Models.DTO.Game;

public class CreateGameRequest
{
    public GameDto Game { get; set; }

    public Identity[] Genres { get; set; }

    public Guid[] Platforms { get; set; }

    public Identity Publisher { get; set; }
}
