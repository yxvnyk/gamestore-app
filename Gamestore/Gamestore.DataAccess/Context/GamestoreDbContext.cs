using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Context;

public class GamestoreDbContext(DbContextOptions<GamestoreDbContext> options) : DbContext(options)
{
    public DbSet<Game> Games { get; set; }

    public DbSet<Genre> Genres { get; set; }

    public DbSet<Platform> Platforms { get; set; }

    public DbSet<Publisher> Publishers { get; set; }

    public DbSet<Comment> Comments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // This method should be empty because all configuraion set by
        // Dependency Injection in Program.cs
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GameGenre>().ToTable("GameGenres");
        modelBuilder.Entity<GamePlatform>().ToTable("GamePlatforms");

        _ = modelBuilder.Entity<GameGenre>().HasKey(gg => new { gg.GameId, gg.GenreId });
        _ = modelBuilder.Entity<GamePlatform>().HasKey(gp => new { gp.GameId, gp.PlatformId });

        // Cascade deleting
        _ = modelBuilder.Entity<Game>()
            .HasMany(g => g.GameGenres)
            .WithOne(g => g.Game)
            .HasForeignKey(g => g.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<Game>()
            .HasMany(g => g.GamePlatforms)
            .WithOne(g => g.Game)
            .HasForeignKey(g => g.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<Game>()
            .HasMany(g => g.Comments)
            .WithOne(c => c.Game)
            .HasForeignKey(c => c.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<Genre>()
            .HasMany(g => g.GameGenres)
            .WithOne(g => g.Genre)
            .HasForeignKey(g => g.GenreId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<Platform>()
            .HasMany(g => g.GamePlatforms)
            .WithOne(g => g.Platform)
            .HasForeignKey(g => g.PlatformId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<Publisher>()
            .HasMany(g => g.Games)
            .WithOne(g => g.Publisher)
            .HasForeignKey(g => g.PublisherId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<Comment>()
            .HasOne(c => c.ParentComment)
            .WithMany(g => g.ChildComments)
            .HasForeignKey(c => c.ParentCommentId)
            .OnDelete(DeleteBehavior.Restrict);

        // data seeding
        _ = modelBuilder.Entity<Platform>().HasData(DataSeeding.GetPlatforms());
        _ = modelBuilder.Entity<Genre>().HasData(DataSeeding.GetGenres());
    }
}
