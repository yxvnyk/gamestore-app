using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Tests;

public class PlatformRepositoryTests
{
    [Fact]
    public async Task CreatePlatformAsync()
    {
        // Arrange
        var options = CreateContextOptions();
        var platform = CreatePlatform();

        // Act
        using (var context = new GamestoreDbContext(options))
        {
            var repository = new PlatformRepository(context);
            await repository.CreatePlatformAsync(platform);
        }

        // Assert
        using (var context = new GamestoreDbContext(options))
        {
            var createdPlatform = await context.Platforms.FirstOrDefaultAsync(g => g.Id == platform.Id);
            Assert.NotNull(createdPlatform);
            Assert.Equal("Test Platform", createdPlatform.Type);
        }
    }

    [Fact]
    public async Task UpdatePlatformAsync_SuccessfulyUpdated()
    {
        // Arrange
        var options = CreateContextOptions();
        var id = Guid.NewGuid();
        var platform = CreatePlatform();
        platform.Id = id;
        var updatedPlatform = CreatePlatform("Updated Platform");
        updatedPlatform.Id = id;

        using (var context = new GamestoreDbContext(options))
        {
            var repository = new PlatformRepository(context);
            await repository.CreatePlatformAsync(platform);
        }

        // Act
        using (var context = new GamestoreDbContext(options))
        {
            var repository = new PlatformRepository(context);
            await repository.UpdatePlatformAsync(updatedPlatform);
            var result = await repository.GetPlatformByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Platform", result.Type);
        }
    }

    [Fact]
    public async Task UpdatePlatformAsync_PlatformNotExist()
    {
        // Arrange
        var options = CreateContextOptions();
        var id = Guid.NewGuid();
        var updatedPlatform = CreatePlatform("Updated Platform");

        // Act
        using var context = new GamestoreDbContext(options);
        var repository = new PlatformRepository(context);
        await repository.UpdatePlatformAsync(updatedPlatform);
        var result = await repository.GetPlatformByIdAsync(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetPlatformByIdAsync()
    {
        // Arrange
        var options = CreateContextOptions();
        var id = Guid.NewGuid();
        var platform = CreatePlatform();
        platform.Id = id;

        using (var context = new GamestoreDbContext(options))
        {
            await SeedPlatform(context, platform);
        }

        // Act
        using (var context = new GamestoreDbContext(options))
        {
            var repository = new PlatformRepository(context);
            var result = await repository.GetPlatformByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Platform", result.Type);
        }
    }

    [Fact]
    public async Task GetAllPlatformsAsync()
    {
        // Arrange
        var options = CreateContextOptions();
        var platforms = new List<Platform>()
        {
            CreatePlatform("Test Platform 1"),
            CreatePlatform("Test Platform 2"),
        };
        using (var context = new GamestoreDbContext(options))
        {
            context.Platforms.AddRange(platforms);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new GamestoreDbContext(options))
        {
            var repository = new PlatformRepository(context);
            var platformCollection = await repository.GetAllPlatformsAsync();
            var result = platformCollection.ToList();

            // Assert
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal($"Test Platform {i + 1}", result[i].Type);
            }
        }
    }

    [Fact]
    public async Task GetPlatformByGameKeyAsync_KeyExist()
    {
        // Arrange
        var options = CreateContextOptions();
        var id = Guid.NewGuid();
        var platforms = new List<Platform>()
        {
            CreatePlatform("Test Platform 1"),
            CreatePlatform("Test Platform 2"),
        };
        var game = CreateGame();
        game.GamePlatforms =
        [
            new GamePlatform()
            {
                Platform = platforms[1],
            },
            new GamePlatform()
            {
                Platform = platforms[2],
            },
        ];
        game.Id = id;

        using (var context = new GamestoreDbContext(options))
        {
            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new GamestoreDbContext(options))
        {
            var repository = new PlatformRepository(context);
            var result = (await repository.GetPlatformsByGameKeyAsync(game.Key)).ToList();

            // Assert
            Assert.NotNull(result);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal($"Test Platform {i + 1}", result[i]!.Type);
            }
        }
    }

    [Fact]
    public async Task GetPlatformsByGameKeyAsync_KeyNotExist()
    {
        // Arrange
        var options = CreateContextOptions();

        // Act
        using var context = new GamestoreDbContext(options);
        var repository = new PlatformRepository(context);
        var result = await repository.GetPlatformsByGameKeyAsync("test-game");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task DeleteByIdAsync_KeyExist()
    {
        // Arrange
        var options = CreateContextOptions();
        var platform = CreatePlatform();

        using (var context = new GamestoreDbContext(options))
        {
            await SeedPlatform(context, platform);
        }

        // Act
        using (var context = new GamestoreDbContext(options))
        {
            var repository = new PlatformRepository(context);
            var result = await repository.DeleteByIdAsync(platform.Id);

            // Assert
            Assert.True(result);
            var notExistEntity = await repository.GetPlatformByIdAsync(platform.Id);
            Assert.Null(notExistEntity);
        }
    }

    [Fact]
    public async Task DeleteByKeyAsync_KeyNotExist()
    {
        // Arrange
        var options = CreateContextOptions();

        // Act
        using var context = new GamestoreDbContext(options);
        var repository = new PlatformRepository(context);
        var result = await repository.DeleteByIdAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task PlatformExistAsync_Exist()
    {
        // Arrange
        var options = CreateContextOptions();
        var platform = CreatePlatform();
        using (var context = new GamestoreDbContext(options))
        {
            await SeedPlatform(context, platform);
        }

        // Act
        using (var context = new GamestoreDbContext(options))
        {
            var repository = new PlatformRepository(context);
            var result = await repository.PlatformExistsAsync(platform.Id);

            // Assert
            Assert.True(result);
        }
    }

    [Fact]
    public async Task PlatformExistAsync_NotExist()
    {
        // Arrange
        var options = CreateContextOptions();

        // Act
        using var context = new GamestoreDbContext(options);
        var repository = new PlatformRepository(context);
        var result = await repository.PlatformExistsAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
    }

    private static async Task SeedPlatform(GamestoreDbContext context, Platform platform)
    {
        context.Platforms.Add(platform);
        await context.SaveChangesAsync();
    }

    private static Platform CreatePlatform(string type = "Test Platform")
    {
        var genre = new Platform()
        {
            Type = type,
        };
        return genre;
    }

    private static Game CreateGame()
    {
        var game = new Game()
        {
            Key = "test-game",
            Description = "This is a test game",
            Name = "Test Game",
        };
        return game;
    }

    private static DbContextOptions<GamestoreDbContext> CreateContextOptions()
    {
        var options = new DbContextOptionsBuilder<GamestoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return options;
    }
}