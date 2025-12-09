using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Entities;

[Index(nameof(Type), IsUnique = true)]
public class Platform
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Type { get; set; }

    public virtual ICollection<GamePlatform> GamePlatforms { get; set; }
}
