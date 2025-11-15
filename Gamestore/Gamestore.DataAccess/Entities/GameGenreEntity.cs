using System.ComponentModel.DataAnnotations.Schema;

namespace Gamestore.DataAccess.Entities;

/// <summary>
/// Represents associative table for Games and Genres many-to-many relationship.
/// GameId and GenreId together form a composite primary key.
/// </summary>
[Table("GameGenres")]
public class GameGenreEntity
{
    public Guid GameId { get; set; }

    public GameEntity Game { get; set; }

    public Guid GenreId { get; set; }

    public GenreEntity Genre { get; set; }
}
