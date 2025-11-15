using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gamestore.DataAccess.Entities;

[Table("Platforms")]
public class PlatformEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Type { get; set; }

    public virtual ICollection<GamePlatformEntity> GamePlatforms { get; set; }
}
