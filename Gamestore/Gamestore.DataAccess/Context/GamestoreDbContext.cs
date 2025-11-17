using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Context;

public class GamestoreDbContext(DbContextOptions<GamestoreDbContext> options) : DbContext(options)
{
    public DbSet<GameEntity> Games { get; set; }

    public DbSet<GenreEntity> Genres { get; set; }

    public DbSet<PlatformEntity> Platforms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<GameGenreEntity>().HasKey(gg => new { gg.GameId, gg.GenreId });
        _ = modelBuilder.Entity<GamePlatformEntity>().HasKey(gp => new { gp.GameId, gp.PlatformId });

        // Cascade deleting
        _ = modelBuilder.Entity<GameEntity>()
            .HasMany(g => g.GameGenres)
            .WithOne(g => g.Game)
            .HasForeignKey(g => g.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<GenreEntity>()
            .HasMany(g => g.GameGenres)
            .WithOne(g => g.Genre)
            .HasForeignKey(g => g.GenreId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<PlatformEntity>()
            .HasMany(g => g.GamePlatforms)
            .WithOne(g => g.Platform)
            .HasForeignKey(g => g.PlatformId)
            .OnDelete(DeleteBehavior.Cascade);

        // data seeding
        _ = modelBuilder.Entity<PlatformEntity>().HasData(DataSeeding.GetPlatforms());
        _ = modelBuilder.Entity<GenreEntity>().HasData(DataSeeding.GetGenres());
    }
}
