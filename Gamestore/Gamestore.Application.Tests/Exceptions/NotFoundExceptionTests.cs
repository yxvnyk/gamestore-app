using Gamestore.Application.Exceptions;

namespace Gamestore.Application.Tests.Exceptions;

public class NotFoundExceptionTests
{
    [Fact]
    public void NotFoundException_DefaultValues_AreSetCorrectly()
    {
        // Arrange & Act
        var exception = new NotFoundException();

        // Assert
        Assert.Equal("Resource not found", exception.Message);
        Assert.Equal(404, exception.StatusCode);
        Assert.Equal("Not found", exception.ErrorCode);
    }

    [Fact]
    public void NotFoundException_CustomValues_AreSetCorrectly()
    {
        // Arrange & Act
        var exception = new NotFoundException("Custom not found exception", "Custom");

        // Assert
        Assert.Equal("Custom not found exception", exception.Message);
        Assert.Equal(404, exception.StatusCode);
        Assert.Equal("Custom", exception.ErrorCode);
    }
}