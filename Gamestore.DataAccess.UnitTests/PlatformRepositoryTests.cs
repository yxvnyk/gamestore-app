using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;

namespace Gamestore.DataAccess.IntegrationTests
{
    public class PlatformRepositoryTests
    {
        private readonly Mock<GamestoreDbContext> _mockContext = new();

        [Fact]
        public async Task CreatePlatformAsync()
        {
            var options = CreateContextOptions();
            var platform = CreatePlatform();

            using (var context = new GamestoreDbContext(options))
            {
                var repository = new PlatformRepository(context);
                await repository.CreatePlatformAsync(platform);
            }

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

            using (var context = new GamestoreDbContext(options))
            {
                var repository = new PlatformRepository(context);
                await repository.UpdatePlatformAsync(updatedPlatform);
                var result = await repository.GetPlatformByIdAsync(id);
                Assert.NotNull(result);
                Assert.Equal("Updated Platform", result.Type);
            }
        }

        [Fact]
        public async Task UpdatePlatformAsync_PlatformNotExist()
        {
            var options = CreateContextOptions();
            var id = Guid.NewGuid();

            var updatedPlatform = CreatePlatform("Updated Platform");

            using (var context = new GamestoreDbContext(options))
            {
                var repository = new PlatformRepository(context);
                await repository.UpdatePlatformAsync(updatedPlatform);
                var result = await repository.GetPlatformByIdAsync(id);
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetPlatformByIdAsync()
        {
            var options = CreateContextOptions();
            var id = Guid.NewGuid();
            var platform = CreatePlatform();
            platform.Id = id;

            using (var context = new GamestoreDbContext(options))
            {
                await SeedPlatform(context, platform);
            }

            using (var context = new GamestoreDbContext(options))
            {
                var repository = new PlatformRepository(context);
                var result = await repository.GetPlatformByIdAsync(id);
                Assert.NotNull(result);
                Assert.Equal("Test Platform", result.Type);
            }

        }

        [Fact]
        public async Task GetAllPlatformsAsync()
        {
            var options = CreateContextOptions();
            var id = Guid.NewGuid();
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

            using (var context = new GamestoreDbContext(options))
            {
                var repository = new PlatformRepository(context);
                var platformCollection = await repository.GetAllPlatformsAsync();
                var result = platformCollection.ToList();
                for (int i = 0; i < result.Count; i++)
                {
                    Assert.Equal($"Test Platform {i+1}", result[i].Type);
                }
            }

        }

        [Fact]
        public async Task GetPlatformByGameKeyAsync_KeyExist()
        {
            var options = CreateContextOptions();
            var id = Guid.NewGuid();
            var platforms = new List<Platform>()
            {
                CreatePlatform("Test Platform 1"),
                CreatePlatform("Test Platform 2"),
            };
            var game = CreateGame();
            game.GamePlatforms = new List<GamePlatform>()
            {
                new GamePlatform()
                {
                    Platform = platforms[0],
                },
                new GamePlatform()
                {
                    Platform = platforms[1],
                },
            };
            game.Id = id;

            using (var context = new GamestoreDbContext(options))
            {
                await context.Games.AddAsync(game);
                await context.SaveChangesAsync();
            }

            using (var context = new GamestoreDbContext(options))
            {
                var repository = new PlatformRepository(context);
                var result = (await repository.GetPlatformsByGameKeyAsync(game.Key)).ToList();
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
            var options = CreateContextOptions();
            var id = Guid.NewGuid();

            using (var context = new GamestoreDbContext(options))
            {
                var repository = new PlatformRepository(context);
                var result = await repository.GetPlatformsByGameKeyAsync("test-game");
                Assert.Empty(result);
            }

        }

        [Fact]
        public async Task DeleteByIdAsync_KeyExist()
        {
            var options = CreateContextOptions();
            var platform = CreatePlatform();

            using (var context = new GamestoreDbContext(options))
            {
                await SeedPlatform(context, platform);
            }

            using (var context = new GamestoreDbContext(options))
            {
                var repository = new PlatformRepository(context);
                var result = await repository.DeleteByIdAsync(platform.Id);
                Assert.True(result);
                var notExistEntity = await repository.GetPlatformByIdAsync(platform.Id);
                Assert.Null(notExistEntity);
            }
        }

        [Fact]
        public async Task DeleteByKeyAsync_KeyNotExist()
        {
            var options = CreateContextOptions();

            using (var context = new GamestoreDbContext(options))
            {
                var repository = new PlatformRepository(context);
                var result = await repository.DeleteByIdAsync(Guid.NewGuid());
                Assert.False(result);
            }
        }

        [Fact]
        public async Task PlatformExistAsync_Exist()
        {
            var options = CreateContextOptions();
            var platform = CreatePlatform();
            using (var context = new GamestoreDbContext(options))
            {
                await SeedPlatform(context, platform);
            }
            using (var context = new GamestoreDbContext(options))
            {
                var repository = new PlatformRepository(context);
                var result = await repository.PlatformExistsAsync(platform.Id);
                Assert.True(result);
            }
        }
        [Fact]
        public async Task PlatformExistAsync_NotExist()
        {
            var options = CreateContextOptions();
            using (var context = new GamestoreDbContext(options))
            {
                var repository = new PlatformRepository(context);
                var result = await repository.PlatformExistsAsync(Guid.NewGuid());
                Assert.False(result);
            }
        }
        
        private async Task SeedPlatform(GamestoreDbContext context, Platform platform)
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
}