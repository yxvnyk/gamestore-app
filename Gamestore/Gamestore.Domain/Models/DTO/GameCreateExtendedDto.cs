namespace Gamestore.Domain.Models.DTO;

public class GameCreateExtendedDto
{
    public GameDto Game { get; set; }

    public Guid[] Genres { get; set; }

    public Guid[] Platforms { get; set; }

    public Guid Publisher { get; set; }
}
