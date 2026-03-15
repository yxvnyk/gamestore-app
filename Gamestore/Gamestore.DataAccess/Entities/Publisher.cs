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

    public bool IsDeleted { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(100)]
    public string? ContactName { get; set; }

    [MaxLength(100)]
    public string? ContactTitle { get; set; }

    [MaxLength(100)]
    public string? Address { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(100)]
    public string? Region { get; set; }

    [MaxLength(100)]
    public string? PostalCode { get; set; }

    [MaxLength(100)]
    public string? Country { get; set; }

    [MaxLength(100)]
    public string? Phone { get; set; }

    [MaxLength(100)]
    public string? Fax { get; set; }

    public string? HomePage { get; set; }

    public ICollection<Game> Games { get; set; }
}
