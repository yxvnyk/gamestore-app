using GameStore.Application.Helpers;
using Gamestore.DataAccess.Repositories.Interfaces;
using Moq;

namespace Gamestore.Application.UnitTests.Helpers;

public class UniqueKeyGeneratorTest
{
    private readonly Mock<IGameRepository> _mockGameRepo = new();

    [Theory]
    [InlineData("key", "key")]
    [InlineData(" key key", "key-key")]
    [InlineData("key:key:", "key-key-")]
    [InlineData("key/", "key-")]
    public async Task GenerateUniqueKeyAsync_UniqueKey_ReturnSameStringInKeyFormat(string name, string expected)
    {
        _mockGameRepo.SetupSequence(repo => repo.GameKeyExistAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        var keyGenerator = new UniqueKeyGenerator(_mockGameRepo.Object);

        var result = await keyGenerator.GenerateUniqueKeyAsync(name);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GenerateUniqueKeyAsync_KeyNotUnique_AppendsIncrementingSuffix()
    {
        string name = "key";
        string expectedKey = "key-3";
        _mockGameRepo.SetupSequence(repo => repo.GameKeyExistAsync("key"))
            .ReturnsAsync(true);
        _mockGameRepo.SetupSequence(repo => repo.GameKeyExistAsync("key-1"))
            .ReturnsAsync(true);
        _mockGameRepo.SetupSequence(repo => repo.GameKeyExistAsync("key-2"))
            .ReturnsAsync(true);
        _mockGameRepo.SetupSequence(repo => repo.GameKeyExistAsync("key-3"))
            .ReturnsAsync(false);

        var keyGenerator = new UniqueKeyGenerator(_mockGameRepo.Object);

        var result = await keyGenerator.GenerateUniqueKeyAsync(name);

        Assert.Equal(expectedKey, result);
    }
}
