using System.ComponentModel.DataAnnotations;
using Gamestore.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Entities;

[Index(nameof(Id), IsUnique = true)]
public class Order
{
    [Key]
    public Guid Id { get; set; }

    public DateTime? DateTime { get; set; }

    [Required]
    public Guid CustomerId { get; set; }

    [Required]
    public OrderStatus Status { get; set; }
}
