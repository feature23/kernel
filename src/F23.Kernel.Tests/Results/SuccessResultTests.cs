using F23.Kernel.Results;

namespace F23.Kernel.Tests.Results;

public class SuccessResultTests
{
    [Fact]
    public void SuccessResult_IsSuccessful()
    {
        // Act
        var result = new SuccessResult();

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void SuccessResultT_IsSuccessful()
    {
        // Arrange
        const int value = 42;

        // Act
        var result = new SuccessResult<int>(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
    }
}
