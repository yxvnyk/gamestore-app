using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Entities;

[Table("Games")]
[Index(nameof(Key), IsUnique = true)]
public class GameEntity
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

    public ICollection<GameGenreEntity> GameGenres { get; set; }

    public ICollection<GamePlatformEntity> GamePlatforms { get; set; }
}
