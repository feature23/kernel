using F23.Kernel.Results;

namespace F23.Kernel.Tests.Results;

public class ValidationFailedResultTests
{
    [Fact]
    public void ValidationFailedResult_ConstructsCorrectly()
    {
        // Arrange
        var errors = new List<ValidationError>
        {
            new("Property1", "Error1"),
            new("Property2", "Error2")
        };

        // Act
        var result = new ValidationFailedResult(errors);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("The operation failed due to validation errors.", result.Message);
        Assert.Equal(errors, result.Errors);
    }

    [Fact]
    public void ValidationFailedResultT_ConstructsCorrectly()
    {
        // Arrange
        var errors = new List<ValidationError>
        {
            new("Property1", "Error1"),
            new("Property2", "Error2")
        };

        // Act
        var result = new ValidationFailedResult<string>(errors);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("The operation failed due to validation errors.", result.Message);
        Assert.Equal(errors, result.Errors);
    }
}
