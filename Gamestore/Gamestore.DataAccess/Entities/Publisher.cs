using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Gamestore.DataAccess.Entities;

[Index(nameof(CompanyName), IsUnique = true)]
public class Publisher
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string CompanyName { get; set; }

    public string? HomePage { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public ICollection<Game> Games { get; set; }
}
