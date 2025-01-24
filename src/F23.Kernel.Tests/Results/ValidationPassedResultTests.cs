using F23.Kernel.Results;

namespace F23.Kernel.Tests.Results;

public class ValidationPassedResultTests
{
    [Fact]
    public void ValidationPassedResult_ConstructsCorrectly()
    {
        // Act
        var result = new ValidationPassedResult();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Validation passed", result.Message);
    }

    [Fact]
    public void ValidationPassedResultT_ConstructsCorrectly()
    {
        // Act
        var result = new ValidationPassedResult<string>();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Validation passed", result.Message);
    }
}
