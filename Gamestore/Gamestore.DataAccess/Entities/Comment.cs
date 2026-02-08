using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Gamestore.DataAccess.Entities;

[Index(nameof(Id), IsUnique = true)]
public class Comment
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [MaxLength(500)]
    public string Body { get; set; }

    public Guid? ParentCommentId { get; set; }

    [Required]
    public Guid GameId { get; set; }

    [Required]
    public bool IsDeleted { get; set; }

    [ForeignKey(nameof(ParentCommentId))]
    public Comment? ParentComment { get; set; }

    [ForeignKey(nameof(GameId))]
    public Game? Game { get; set; }

    public ICollection<Comment> ChildComments { get; set; } = [];
}
