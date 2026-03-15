using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    [Required]
    public double Price { get; set; }

    [Required]
    public int UnitsInStock { get; set; }

    [Required]
    public int Discount { get; set; }

    [Required]
    public Guid PublisherId { get; set; }

    [ForeignKey("PublisherId")]
    public Publisher? Publisher { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool IsDeleted { get; set; }

    [MaxLength(20)]
    public string? QuantityPerUnit { get; set; }

    public int? UnitsOnOrder { get; set; }

    public int? ReorderLevel { get; set; }

    public bool? Discontinued { get; set; }

    public ICollection<GameGenre> GameGenres { get; set; }

    public ICollection<GamePlatform> GamePlatforms { get; set; }

    public ICollection<Comment> Comments { get; set; }
}
