using F23.Kernel.Results;

namespace F23.Kernel.Tests.Results;

public class UnauthorizedResultTests
{
    [Fact]
    public void UnauthorizedResult_IsNotSuccess()
    {
        // Arrange
        const string message = "YOU SHALL NOT PASS";

        // Act
        var result = new UnauthorizedResult(message);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(message, result.Message);
    }

    [Fact]
    public void UnauthorizedResultT_IsNotSuccess()
    {
        // Arrange
        const string message = "YOU SHALL NOT PASS";

        // Act
        var result = new UnauthorizedResult<string>(message);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(message, result.Message);
    }
}
