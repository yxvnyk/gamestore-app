using Gamestore.DataAccess.Entities;

namespace Gamestore.DataAccess.Wrappers;

public class GameWithStats
{
    public Game Game { get; set; }

    public int CommentCount { get; set; }

    public DateTime? CreatedDate { get; set; }
}
