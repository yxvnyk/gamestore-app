using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Tests;

public class GameRepositoryTests
{
    [Fact]
    public async Task CreateGameAsync()
    {
        var options = CreateContextOptions();
        var game = CreateGame();

        using (var context = new GamestoreDbContext(options))
        {
            var repository = new GameRepository(context);
            await repository.CreateGameAsync(game);
        }

        using (var context = new GamestoreDbContext(options))
        {
            var createdGame = await context.Games.FirstOrDefaultAsync(g => g.Key == "test-game");
            Assert.NotNull(createdGame);
            Assert.Equal("Test Game", createdGame.Name);
            Assert.Equal("This is a test game", createdGame.Description);
        }
    }

    [Fact]
    public async Task UpdateGameAsync_SuccessfulyUpdated()
    {
        var options = CreateContextOptions();
        var id = Guid.NewGuid();
        var game = CreateGame();
        game.Id = id;
        var updatedGame = new Game()
        {
            Key = "updated-game",
            Description = "This is a updated game",
            Name = "Updated Game",
            Id = id,
        };

        using (var context = new GamestoreDbContext(options))
        {
            var repository = new GameRepository(context);
            await repository.CreateGameAsync(game);
        }

        using (var context = new GamestoreDbContext(options))
        {
            var repository = new GameRepository(context);
            await repository.UpdateGameAsync(updatedGame);
            var result = await repository.GetGameByIdAsync(id);
            Assert.NotNull(result);
            Assert.Equal("Updated Game", result.Name);
            Assert.Equal("updated-game", result.Key);
            Assert.Equal("This is a updated game", result.Description);
        }
    }

    [Fact]
    public async Task UpdateGameAsync_GameNotExist()
    {
        var options = CreateContextOptions();
        var id = Guid.NewGuid();

        var updatedGame = new Game()
        {
            Key = "updated-game",
            Description = "This is a updated game",
            Name = "Updated Game",
            Id = id,
        };

        using var context = new GamestoreDbContext(options);
        var repository = new GameRepository(context);
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.UpdateGameAsync(updatedGame));
    }

    [Fact]
    public async Task GameKeyExistAsync()
    {
        var options = CreateContextOptions();
        var game = CreateGame();

        using (var context = new GamestoreDbContext(options))
        {
            await SeedGame(context, game);
        }

        using (var context = new GamestoreDbContext(options))
        {
            var repository = new GameRepository(context);
            var result = await repository.GetGameByKeyAsync(game.Key);
            Assert.NotNull(result);
            Assert.Equal("Test Game", result.Name);
        }
    }

    [Fact]
    public async Task GetGameByIdAsync()
    {
        var options = CreateContextOptions();
        var id = Guid.NewGuid();
        var game = CreateGame();
        game.Id = id;

        using (var context = new GamestoreDbContext(options))
        {
            await SeedGame(context, game);
        }

        using (var context = new GamestoreDbContext(options))
        {
            var repository = new GameRepository(context);
            var result = await repository.GetGameByIdAsync(id);
            Assert.NotNull(result);
            Assert.Equal("Test Game", result.Name);
            Assert.Equal("This is a test game", result.Description);
            Assert.Equal("test-game", result.Key);
            Assert.Equal(id, result.Id);
        }
    }

    [Fact]
    public async Task GetGameWithJoinsAsync_ReturnGameWithPlatformsAndGenres()
    {
        var options = CreateContextOptions();
        var platformIds = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        var genreIds = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

        var game = CreateGame();
        game.GamePlatforms = [.. platformIds.Select(id => new GamePlatform { PlatformId = id })];
        game.GameGenres = [.. genreIds.Select(id => new GameGenre { GenreId = id })];

        using (var context = new GamestoreDbContext(options))
        {
            await SeedGame(context, game);
        }

        using (var context = new GamestoreDbContext(options))
        {
            var repository = new GameRepository(context);
            var result = await repository.GetGameWithJoinsAsync(game.Id);
            Assert.NotNull(result);
            Assert.Equal("Test Game", result.Name);
            Assert.Equal("This is a test game", result.Description);
            Assert.Equal("test-game", result.Key);
            Assert.Equal(game.Id, result.Id);

            Assert.Equal(platformIds.Length, result.GamePlatforms.Count);
            for (int i = 0; i < result.GamePlatforms.Count; i++)
            {
                Assert.Equal(game.GamePlatforms.ToList()[i].PlatformId, result.GamePlatforms.ToList()[i].PlatformId);
            }

            Assert.Equal(genreIds.Length, result.GameGenres.Count);
            for (int i = 0; i < result.GameGenres.Count; i++)
            {
                Assert.Equal(game.GameGenres.ToList()[i].GenreId, result.GameGenres.ToList()[i].GenreId);
            }
        }
    }

    [Fact]
    public async Task GetAllGamesAsync_()
    {
        var options = CreateContextOptions();
        var games = new List<Game>()
        {
            new()
            {
                Key = "test-game-1",
                Description = "This is test game 1",
                Name = "Test Game 1",
            },
            new()
            {
                Key = "test-game-2",
                Description = "This is test game 2",
                Name = "Test Game 2",
            },
        };
        using (var context = new GamestoreDbContext(options))
        {
            context.Games.AddRange(games);
            await context.SaveChangesAsync();
        }

        using (var context = new GamestoreDbContext(options))
        {
            var repository = new GameRepository(context);
            var gamesCollection = await repository.GetAllGamesAsync();
            var result = gamesCollection.ToList();
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal($"Test Game {i + 1}", result[i].Name);
                Assert.Equal($"This is test game {i + 1}", result[i].Description);
                Assert.Equal($"test-game-{i + 1}", result[i].Key);
            }
        }
    }

    [Fact]
    public async Task GetTotalGamesCountAsync_ZeroGamesInDatabase()
    {
        var options = CreateContextOptions();

        using var context = new GamestoreDbContext(options);
        var repository = new GameRepository(context);
        var count = await repository.GetTotalGamesCountAsync();
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task GetTotalGamesCountAsync_DbContainSameGames()
    {
        var options = CreateContextOptions();
        var games = new List<Game>()
        {
            new()
            {
                Key = "test-game-1",
                Description = "This is test game 1",
                Name = "Test Game 1",
            },
            new()
            {
                Key = "test-game-2",
                Description = "This is test game 2",
                Name = "Test Game 2",
            },
        };
        using (var context = new GamestoreDbContext(options))
        {
            context.Games.AddRange(games);
            await context.SaveChangesAsync();
        }

        using (var context = new GamestoreDbContext(options))
        {
            var repository = new GameRepository(context);
            var count = await repository.GetTotalGamesCountAsync();
            Assert.Equal(games.Count, count);
        }
    }

    [Fact]
    public async Task GetGameByPlatformAsync()
    {
        var options = CreateContextOptions();
        var platformId = Guid.NewGuid();
        var platform = new Platform()
        {
            Id = platformId,
            Type = "Test Platform",
        };
        var game = CreateGame();
        game.GamePlatforms =
        [
            new GamePlatform()
            {
                PlatformId = platformId,
            }
        ];

        using (var context = new GamestoreDbContext(options))
        {
            context.Platforms.Add(platform);
            context.Games.Add(game);
            await context.SaveChangesAsync();
        }

        using (var context = new GamestoreDbContext(options))
        {
            var repository = new GameRepository(context);
            var result = await repository.GetGamesByPlatformAsync(platformId);
            foreach (var item in result)
            {
                Assert.Equal("Test Game", item.Name);
                Assert.Equal("This is a test game", item.Description);
                Assert.Equal("test-game", item.Key);
            }
        }
    }

    [Fact]
    public async Task GetGameByGenreAsync_ReturnGamesList()
    {
        var options = CreateContextOptions();
        var genreId = Guid.NewGuid();
        var genre = new Genre()
        {
            Id = genreId,
            Name = "Test Genre",
        };
        var game = CreateGame();
        game.GameGenres =
        [
            new GameGenre()
            {
                GenreId = genreId,
            }
        ];

        using (var context = new GamestoreDbContext(options))
        {
            context.Genres.Add(genre);
            context.Games.Add(game);
            await context.SaveChangesAsync();
        }

        using (var context = new GamestoreDbContext(options))
        {
            var repository = new GameRepository(context);
            var result = await repository.GetGamesByGenreAsync(genreId);
            foreach (var item in result)
            {
                Assert.Equal("Test Game", item.Name);
                Assert.Equal("This is a test game", item.Description);
                Assert.Equal("test-game", item.Key);
            }
        }
    }

    [Fact]
    public async Task GetGameByGenreAsync_NoGamesWithThisGenre()
    {
        var options = CreateContextOptions();
        var genreId = Guid.NewGuid();

        using var context = new GamestoreDbContext(options);
        var repository = new GameRepository(context);
        var result = await repository.GetGamesByGenreAsync(genreId);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetGameByKeyAsync_KeyExist()
    {
        var options = CreateContextOptions();
        var id = Guid.NewGuid();
        var game = CreateGame();
        game.Id = id;

        using (var context = new GamestoreDbContext(options))
        {
            await SeedGame(context, game);
        }

        using (var context = new GamestoreDbContext(options))
        {
            var repository = new GameRepository(context);
            var result = await repository.GetGameByKeyAsync(game.Key);
            Assert.NotNull(result);
            Assert.Equal("Test Game", result.Name);
            Assert.Equal("This is a test game", result.Description);
            Assert.Equal("test-game", result.Key);
            Assert.Equal(id, result.Id);
        }
    }

    [Fact]
    public async Task GetGameByKeyAsync_KeyNotExist()
    {
        var options = CreateContextOptions();

        using var context = new GamestoreDbContext(options);
        var repository = new GameRepository(context);
        var result = await repository.GetGameByKeyAsync("test-game");
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteByKeyAsync_KeyExist()
    {
        var options = CreateContextOptions();
        var game = new Game()
        {
            Key = "test-game",
            Description = "This is a test game",
            Name = "Test Game",
        };

        using (var context = new GamestoreDbContext(options))
        {
            await SeedGame(context, game);
        }

        using (var context = new GamestoreDbContext(options))
        {
            var repository = new GameRepository(context);
            var result = await repository.DeleteByKeyAsync(game.Key);
            Assert.True(result);
            var notExistEntity = await repository.GetGameByKeyAsync(game.Key);
            Assert.Null(notExistEntity);
        }
    }

    [Fact]
    public async Task DeleteByKeyAsync_KeyNotExist()
    {
        var options = CreateContextOptions();

        using var context = new GamestoreDbContext(options);
        var repository = new GameRepository(context);
        var result = await repository.DeleteByKeyAsync("test-game");
        Assert.False(result);
    }

    private static async Task SeedGame(GamestoreDbContext context, Game game)
    {
        context.Games.Add(game);
        await context.SaveChangesAsync();
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