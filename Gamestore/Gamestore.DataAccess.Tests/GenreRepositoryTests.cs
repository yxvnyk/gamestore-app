using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Tests;

public class GenreRepositoryTests
{
    [Fact]
    public async Task CreateGenreAsync()
    {
        // Arrange
        var options = CreateContextOptions();
        var genre = CreateGenre();

        // Act
        using (var context = new GamestoreDbContext(options))
        {
            var repository = new GenreRepository(context);
            await repository.CreateGenreAsync(genre);
        }

        // Assert
        using (var context = new GamestoreDbContext(options))
        {
            var createdGame = await context.Genres.FirstOrDefaultAsync(g => g.Id == genre.Id);
            Assert.NotNull(createdGame);
            Assert.Equal("Test Genre", createdGame.Name);
            Assert.Equal(genre.ParentGenreId, createdGame.ParentGenreId);
        }
    }

    [Fact]
    public async Task UpdateGameAsync_SuccessfulyUpdated()
    {
        // Arrange
        var options = CreateContextOptions();
        var id = Guid.NewGuid();
        var genre = CreateGenre();
        genre.Id = id;
        var updatedGenre = new Genre()
        {
            Name = "Updated Genre",
            Id = id,
            ParentGenreId = Guid.NewGuid(),
        };

        using (var context = new GamestoreDbContext(options))
        {
            var repository = new GenreRepository(context);
            await repository.CreateGenreAsync(genre);
        }

        // Act
        using (var context = new GamestoreDbContext(options))
        {
            var repository = new GenreRepository(context);
            await repository.UpdateGenreAsync(updatedGenre);
            var result = await repository.GetGenreByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Genre", result.Name);
            Assert.Equal(updatedGenre.ParentGenreId, result.ParentGenreId);
        }
    }

    [Fact]
    public async Task UpdateGenreAsync_GenreNotExist()
    {
        // Arrange
        var options = CreateContextOptions();
        var id = Guid.NewGuid();

        var updatedGenre = new Genre()
        {
            Name = "Updated Genre",
            Id = id,
            ParentGenreId = Guid.NewGuid(),
        };

        // Act & Assert
        using var context = new GamestoreDbContext(options);
        var repository = new GenreRepository(context);
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.UpdateGenreAsync(updatedGenre));
    }

    [Fact]
    public async Task GetGenreByIdAsync()
    {
        // Arrange
        var options = CreateContextOptions();
        var id = Guid.NewGuid();
        var genre = CreateGenre();
        genre.Id = id;

        using (var context = new GamestoreDbContext(options))
        {
            await SeedGenre(context, genre);
        }

        // Act
        using (var context = new GamestoreDbContext(options))
        {
            var repository = new GenreRepository(context);
            var result = await repository.GetGenreByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Genre", result.Name);
            Assert.Equal(genre.ParentGenreId, result.ParentGenreId);
        }
    }

    [Fact]
    public async Task GetAllGenresAsync()
    {
        // Arrange
        var options = CreateContextOptions();
        var genres = new List<Genre>()
        {
            CreateGenre("Test Genre 1"),
            CreateGenre("Test Genre 2"),
        };
        using (var context = new GamestoreDbContext(options))
        {
            context.Genres.AddRange(genres);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new GamestoreDbContext(options))
        {
            var repository = new GenreRepository(context);
            var genresCollection = await repository.GetAllGenresAsync();
            var result = genresCollection.ToList();

            // Assert
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal($"Test Genre {i + 1}", result[i].Name);
                Assert.Equal(genres[i].ParentGenreId, result[i].ParentGenreId);
            }
        }
    }

    [Fact]
    public async Task GetGenresByGameKeyAsync_KeyExist()
    {
        // Arrange
        var options = CreateContextOptions();
        var id = Guid.NewGuid();
        var genres = new List<Genre>()
        {
            CreateGenre("Test Genre 1"),
            CreateGenre("Test Genre 2"),
        };
        var game = CreateGame();
        game.GameGenres =
        [
            new GameGenre()
            {
                Genre = genres[0],
            },
            new GameGenre()
            {
                Genre = genres[1],
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
            var repository = new GenreRepository(context);
            var result = (await repository.GetGenresByGameKeyAsync(game.Key)).ToList();

            // Assert
            Assert.NotNull(result);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal($"Test Genre {i + 1}", result[i]!.Name);
                Assert.Equal(genres[i].ParentGenreId, result[i]!.ParentGenreId);
            }
        }
    }

    [Fact]
    public async Task GetGenresByGameKeyAsync_KeyNotExist()
    {
        // Arrange
        var options = CreateContextOptions();

        // Act
        using var context = new GamestoreDbContext(options);
        var repository = new GenreRepository(context);
        var result = await repository.GetGenresByGameKeyAsync("test-game");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task DeleteByIdAsync_KeyExist()
    {
        // Arrange
        var options = CreateContextOptions();
        var genre = CreateGenre();

        using (var context = new GamestoreDbContext(options))
        {
            await SeedGenre(context, genre);
        }

        // Act
        using (var context = new GamestoreDbContext(options))
        {
            var repository = new GenreRepository(context);
            var result = await repository.DeleteByIdAsync(genre.Id);

            // Assert
            Assert.True(result);
            var notExistEntity = await repository.GetGenreByIdAsync(genre.Id);
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
        var repository = new GenreRepository(context);
        var result = await repository.DeleteByIdAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GenreExistAsync_Exist()
    {
        // Arrange
        var options = CreateContextOptions();
        var genre = CreateGenre();
        using (var context = new GamestoreDbContext(options))
        {
            await SeedGenre(context, genre);
        }

        // Act
        using (var context = new GamestoreDbContext(options))
        {
            var repository = new GenreRepository(context);
            var result = await repository.GenreExistsAsync(genre.Id);

            // Assert
            Assert.True(result);
        }
    }

    [Fact]
    public async Task GenreExistAsync_NotExist()
    {
        // Arrange
        var options = CreateContextOptions();

        // Act
        using var context = new GamestoreDbContext(options);
        var repository = new GenreRepository(context);
        var result = await repository.GenreExistsAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetGenresByParentId_ParentExist()
    {
        // Arrange
        var parentId = Guid.NewGuid();
        var options = CreateContextOptions();
        var parentGenre = CreateGenre("Parent Genre");
        parentGenre.Id = parentId;

        var childGenres = new List<Genre>()
        {
            CreateGenre("Child Genre 1"),
            CreateGenre("Child Genre 2"),
        };
        childGenres[0].ParentGenreId = parentId;
        childGenres[1].ParentGenreId = parentId;

        using (var context = new GamestoreDbContext(options))
        {
            context.Genres.Add(parentGenre);
            context.Genres.AddRange(childGenres);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new GamestoreDbContext(options))
        {
            var repository = new GenreRepository(context);
            var result = (await repository.GetGenresByParentIdAsync(parentId)).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal($"Child Genre {i + 1}", result[i].Name);
                Assert.Equal(parentId, result[i].ParentGenreId);
            }
        }
    }

    [Fact]
    public async Task GetGenresByParentId_ParentNotExist()
    {
        // Arrange
        var options = CreateContextOptions();

        // Act
        using var context = new GamestoreDbContext(options);
        var repository = new GenreRepository(context);
        var result = await repository.GetGenresByParentIdAsync(Guid.NewGuid());

        // Assert
        Assert.Empty(result);
    }

    private static async Task SeedGenre(GamestoreDbContext context, Genre genre)
    {
        context.Genres.Add(genre);
        await context.SaveChangesAsync();
    }

    private static Genre CreateGenre(string name = "Test Genre")
    {
        var genre = new Genre()
        {
            Name = name,
            ParentGenreId = Guid.NewGuid(),
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