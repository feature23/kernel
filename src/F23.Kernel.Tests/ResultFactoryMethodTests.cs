using F23.Kernel.Results;

namespace F23.Kernel.Tests;

public class ResultFactoryMethodTests
{
    [Fact]
    public void Success_ReturnsSuccessResult()
    {
        // Act
        var result = Result.Success();

        // Assert
        Assert.IsType<SuccessResult>(result);
    }

    [Fact]
    public void SuccessT_ReturnsSuccessResult()
    {
        // Arrange
        var value = new TestResultContent();

        // Act
        var result = Result<TestResultContent>.Success(value);

        // Assert
        var successResult = Assert.IsType<SuccessResult<TestResultContent>>(result);
        Assert.Equal(value, successResult.Value);
    }

    [Fact]
    public void Aggregate_ReturnsAggregateResult()
    {
        // Arrange
        var results = new List<Result> { Result.Success(), Result.Success() };

        // Act
        var result = Result.Aggregate(results);

        // Assert
        var aggregateResult = Assert.IsType<AggregateResult>(result);
        Assert.Equal(results, aggregateResult.Results);
    }

    [Fact]
    public void Unauthorized_ReturnsUnauthorizedResult()
    {
        // Arrange
        const string message = "YOU SHALL NOT PASS";

        // Act
        var result = Result.Unauthorized(message);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedResult>(result);
        Assert.Equal(message, unauthorizedResult.Message);
    }

    [Fact]
    public void UnauthorizedT_ReturnsUnauthorizedResult()
    {
        // Arrange
        const string message = "YOU SHALL NOT PASS";

        // Act
        var result = Result<TestResultContent>.Unauthorized(message);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedResult<TestResultContent>>(result);
        Assert.Equal(message, unauthorizedResult.Message);
    }

    [Fact]
    public void ValidationFailed_ReturnsValidationFailedResult()
    {
        // Arrange
        var errors = new List<ValidationError> { new("key", "message") };

        // Act
        var result = Result.ValidationFailed(errors);

        // Assert
        var validationFailedResult = Assert.IsType<ValidationFailedResult>(result);
        Assert.Equal(errors, validationFailedResult.Errors);
    }

    [Fact]
    public void ValidationFailedT_ReturnsValidationFailedResult()
    {
        // Arrange
        var errors = new List<ValidationError> { new("key", "message") };

        // Act
        var result = Result<TestResultContent>.ValidationFailed(errors);

        // Assert
        var validationFailedResult = Assert.IsType<ValidationFailedResult<TestResultContent>>(result);
        Assert.Equal(errors, validationFailedResult.Errors);
    }

    [Fact]
    public void ValidationFailed_SingleError_ReturnsValidationFailedResult()
    {
        // Arrange
        const string key = "key";
        const string message = "message";

        // Act
        var result = Result.ValidationFailed(key, message);

        // Assert
        var validationFailedResult = Assert.IsType<ValidationFailedResult>(result);
        var error = Assert.Single(validationFailedResult.Errors);
        Assert.Equal(key, error.Key);
        Assert.Equal(message, error.Message);
    }

    [Fact]
    public void ValidationFailedT_SingleError_ReturnsValidationFailedResult()
    {
        // Arrange
        const string key = "key";
        const string message = "message";

        // Act
        var result = Result<TestResultContent>.ValidationFailed(key, message);

        // Assert
        var validationFailedResult = Assert.IsType<ValidationFailedResult<TestResultContent>>(result);
        var error = Assert.Single(validationFailedResult.Errors);
        Assert.Equal(key, error.Key);
        Assert.Equal(message, error.Message);
    }

    [Fact]
    public void PreconditionFailed_ReturnsPreconditionFailedResult()
    {
        // Arrange
        const PreconditionFailedReason reason = PreconditionFailedReason.Conflict;
        const string message = "Precondition failed";

        // Act
        var result = Result.PreconditionFailed(reason, message);

        // Assert
        var preconditionFailedResult = Assert.IsType<PreconditionFailedResult>(result);
        Assert.Equal(reason, preconditionFailedResult.Reason);
        Assert.Equal(message, preconditionFailedResult.Message);
    }

    [Fact]
    public void PreconditionFailedT_ReturnsPreconditionFailedResult()
    {
        // Arrange
        const PreconditionFailedReason reason = PreconditionFailedReason.Conflict;
        const string message = "Precondition failed";

        // Act
        var result = Result<TestResultContent>.PreconditionFailed(reason, message);

        // Assert
        var preconditionFailedResult = Assert.IsType<PreconditionFailedResult<TestResultContent>>(result);
        Assert.Equal(reason, preconditionFailedResult.Reason);
        Assert.Equal(message, preconditionFailedResult.Message);
    }

    // TODO.JB - test mapping methods

    private class TestResultContent;
}
