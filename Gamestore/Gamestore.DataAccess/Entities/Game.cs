using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Entities;

[Index(nameof(Key), IsUnique = true)]
public class Game
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [MaxLength(100)]
    public string Key { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public ICollection<GameGenre> GameGenres { get; set; }

    public ICollection<GamePlatform> GamePlatforms { get; set; }
}
