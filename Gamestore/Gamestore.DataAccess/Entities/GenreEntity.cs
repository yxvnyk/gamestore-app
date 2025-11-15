using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gamestore.DataAccess.Entities;

[Table("Genres")]
public class GenreEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    public Guid? ParentGenreId { get; set; }

    [ForeignKey("ParentGenreId")]
    public GenreEntity? ParentGenre { get; set; }

    public ICollection<GameGenreEntity> GameGenres { get; set; }
}
