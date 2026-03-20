using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Gamestore.DataAccess.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Genre
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    public string? Description { get; set; }

    public Guid? ParentGenreId { get; set; }

    [ForeignKey("ParentGenreId")]
    public Genre? ParentGenre { get; set; }

    public string? Picture { get; set; }

    public int? LegacyId { get; set; }

    public bool IsDeleted { get; set; }

    public ICollection<GameGenre> GameGenres { get; set; }
}
