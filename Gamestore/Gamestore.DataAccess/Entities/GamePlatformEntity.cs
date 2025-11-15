using System.ComponentModel.DataAnnotations.Schema;

namespace Gamestore.DataAccess.Entities;

/// <summary>
/// Represents associative table Games and Platform many-to-many relationship.
/// GameId and PlatformId together form a composite primary key.
/// </summary>
[Table("GamePlatforms")]
public class GamePlatformEntity
{
    public Guid GameId { get; set; }

    public GameEntity Game { get; set; }

    public Guid PlatformId { get; set; }

    public PlatformEntity Platform { get; set; }
}
