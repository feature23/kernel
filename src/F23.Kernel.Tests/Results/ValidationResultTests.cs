using F23.Kernel.Results;

namespace F23.Kernel.Tests.Results;

public class ValidationResultTests
{
    [Fact]
    public void ValidationResult_Failed_SingleError_ConstructsResult()
    {
        // Arrange
        var error = new ValidationError("key", "message");

        // Act
        var result = ValidationResult.Failed(error);

        // Assert
        Assert.False(result.IsSuccess);
        var failedResult = Assert.IsType<ValidationFailedResult>(result);
        var actual = Assert.Single(failedResult.Errors);
        Assert.Equal(error, actual);
    }

    [Fact]
    public void ValidationResult_Failed_SingleErrorKeyMessage_ConstructsResult()
    {
        // Arrange
        const string key = "key";
        const string message = "message";

        // Act
        var result = ValidationResult.Failed(key, message);

        // Assert
        Assert.False(result.IsSuccess);
        var failedResult = Assert.IsType<ValidationFailedResult>(result);
        var actual = Assert.Single(failedResult.Errors);
        Assert.Equal(key, actual.Key);
        Assert.Equal(message, actual.Message);
    }

    [Fact]
    public void ValidationResult_Failed_MultipleErrors_ConstructsResult()
    {
        // Arrange
        var errors = new[]
        {
            new ValidationError("key1", "message1"),
            new ValidationError("key2", "message2"),
            new ValidationError("key3", "message3")
        };

        // Act
        var result = ValidationResult.Failed(errors);

        // Assert
        Assert.False(result.IsSuccess);
        var failedResult = Assert.IsType<ValidationFailedResult>(result);
        Assert.Equal(errors, failedResult.Errors);
    }

    [Fact]
    public void ValidationResult_Passed_ConstructsResult()
    {
        // Act
        var result = ValidationResult.Passed();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.IsType<ValidationPassedResult>(result);
    }

    [Fact]
    public void ValidationResultT_Failed_MultipleErrors_ConstructsResult()
    {
        // Arrange
        var errors = new[]
        {
            new ValidationError("key1", "message1"),
            new ValidationError("key2", "message2"),
            new ValidationError("key3", "message3")
        };

        // Act
        var result = ValidationResult<string>.Failed(errors);

        // Assert
        Assert.False(result.IsSuccess);
        var failedResult = Assert.IsType<ValidationFailedResult<string>>(result);
        Assert.Equal(errors, failedResult.Errors);
    }

    [Fact]
    public void ValidationResultT_Passed_ConstructsResult()
    {
        // Act
        var result = ValidationResult<string>.Passed();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.IsType<ValidationPassedResult<string>>(result);
    }
}
