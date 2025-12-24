using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.WebApi.Helpers.Middleware;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace Gamestore.WebApi.UnitTests.Helpers.Middleware;

public class TotalGamesHeaderMiddlewareTests
{
    [Fact]
    public async Task GetTotalGamesCountAsyncReturnsValueFromRepository()
    {
        // Arrange
        var totalGames = 42;
        var gameRepoMock = new Mock<IGameRepository>();
        gameRepoMock.Setup(r => r.GetTotalGamesCountAsync()).ReturnsAsync(totalGames);

        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var middleware = new TotalGamesHeaderMiddleware(gameRepoMock.Object, memoryCache);

        // Act
        var result = await middleware.GetTotalGamesCountAsync();

        // Assert
        Assert.Equal(totalGames, result);

        gameRepoMock.Verify(r => r.GetTotalGamesCountAsync(), Times.Once);
    }
}