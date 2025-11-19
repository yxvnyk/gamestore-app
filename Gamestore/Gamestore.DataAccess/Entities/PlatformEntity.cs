using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Entities;

[Table("Platforms")]
[Index(nameof(Type), IsUnique = true)]
public class PlatformEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Type { get; set; }

    public virtual ICollection<GamePlatformEntity> GamePlatforms { get; set; }
}
