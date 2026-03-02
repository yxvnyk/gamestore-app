using System.ComponentModel.DataAnnotations;
using Gamestore.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Entities;

[Index(nameof(Id), IsUnique = true)]
public class Order
{
    [Key]
    public Guid Id { get; set; }

    public int? LegacyOrderId { get; set; }

    public DateTime? DateTime { get; set; }

    [Required]
    public Guid CustomerId { get; set; }

    [Required]
    public OrderStatus Status { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime? RequiredDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public int? ShipVia { get; set; }

    public decimal? Freight { get; set; }

    public string? ShipName { get; set; }

    public string? ShipAddress { get; set; }

    public string? ShipCity { get; set; }

    public string? ShipRegion { get; set; }

    public string? ShipPostalCode { get; set; }

    public string? ShipCountry { get; set; }
}
