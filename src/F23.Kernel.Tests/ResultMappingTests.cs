using F23.Kernel.Results;
using F23.Kernel.Tests.Mocks;

namespace F23.Kernel.Tests;

public class ResultMappingTests
{
    [Fact]
    public void Map_Success_ReturnsSuccessResult()
    {
        // Arrange
        var result = Result<TestResultContent>.Success(new TestResultContent());

        // Act
        var mappedResult = result.Map(r => 42);

        // Assert
        var typedResult = Assert.IsType<SuccessResult<int>>(mappedResult);
        Assert.Equal(42, typedResult.Value);
    }

    [Fact]
    public void MapFailure_SuccessResult_Throws()
    {
        // Arrange
        var result = Result<TestResultContent>.Success(new TestResultContent());

        // Act
        void Act() => result.MapFailure<int>();

        // Assert
        Assert.Throws<InvalidOperationException>(Act);
    }

    [Fact]
    public void MapFailure_ValidationFailedResult_MapsCorrectly()
    {
        // Arrange
        var result = Result<TestResultContent>.ValidationFailed("key", "message");

        // Act
        var mappedResult = result.Map(r => 42);

        // Assert
        var typedResult = Assert.IsType<ValidationFailedResult<int>>(mappedResult);
        var error = Assert.Single(typedResult.Errors);
        Assert.Equal("key", error.Key);
        Assert.Equal("message", error.Message);
    }

    [Fact]
    public void MapFailure_UnauthorizedResult_MapsCorrectly()
    {
        // Arrange
        var result = Result<TestResultContent>.Unauthorized("YOU SHALL NOT PASS");

        // Act
        var mappedResult = result.Map(r => 42);

        // Assert
        var typedResult = Assert.IsType<UnauthorizedResult<int>>(mappedResult);
        Assert.Equal("YOU SHALL NOT PASS", typedResult.Message);
    }

    [Fact]
    public void MapFailure_PreconditionFailedResult_MapsCorrectly()
    {
        // Arrange
        var result = Result<TestResultContent>.PreconditionFailed(PreconditionFailedReason.NotFound, "message");

        // Act
        var mappedResult = result.Map(r => 42);

        // Assert
        var typedResult = Assert.IsType<PreconditionFailedResult<int>>(mappedResult);
        Assert.Equal(PreconditionFailedReason.NotFound, typedResult.Reason);
    }

    [Fact]
    public void MapFailure_UnknownType_Throws()
    {
        // Arrange
        var result = new UnknownResult();

        // Act
        void Act() => result.MapFailure<int>();

        // Assert
        Assert.Throws<InvalidOperationException>(Act);
    }

    [Fact]
    public void GenericSuccessResult_IsConvertedTo_NonGenericSuccessResult()
    {
        var genericResult = Result<int>.Success(42);
        var plainResult = genericResult.Map();

        Assert.IsType<SuccessResult>(plainResult);
    }

    [Fact]
    public void GenericValidationFailedResult_IsConvertedTo_NonGenericValidationFailedResult()
    {
        var genericResult = Result<int>.ValidationFailed("key", "message");
        var plainResult = genericResult.Map();

        if (plainResult is not ValidationFailedResult validationFailedResult)
        {
            throw new InvalidOperationException("The converted result is not a ValidationFailedResult.");
        }

        Assert.Single(validationFailedResult.Errors);
        Assert.Equal("key", validationFailedResult.Errors.First().Key);
        Assert.Equal("message", validationFailedResult.Errors.First().Message);
    }

    [Fact]
    public void GenericValidationPassedResult_IsConvertedTo_NonGenericValidationPassedResult()
    {
        var genericResult = new ValidationPassedResult<int>();
        var plainResult = genericResult.Map();

        if (plainResult is not ValidationPassedResult validationFailedResult)
        {
            throw new InvalidOperationException("The converted result is not a ValidationFailedResult.");
        }

        Assert.Equal("Validation passed", validationFailedResult.Message);
    }

    [Fact]
    public void GenericUnauthorizedResult_IsConvertedTo_NonGenericUnauthorizedResult()
    {
        var genericResult = Result<int>.Unauthorized("Something went wrong.");
        var plainResult = genericResult.Map();

        if (plainResult is not UnauthorizedResult unauthorizedResult)
        {
            throw new InvalidOperationException("The converted result is not an UnauthorizedResult.");
        }

        Assert.Equal("Something went wrong.", unauthorizedResult.Message);
    }

    [Fact]
    public void GenericPreconditionFailedResult_IsConvertedTo_NonGenericPreconditionFailedResult()
    {
        var genericResult = Result<int>.PreconditionFailed(PreconditionFailedReason.NotFound);
        var plainResult = genericResult.Map();

        if (plainResult is not PreconditionFailedResult preconditionFailedResult)
        {
            throw new InvalidOperationException("The converted result is not a PreconditionFailedResult.");
        }

        Assert.Equal(PreconditionFailedReason.NotFound, preconditionFailedResult.Reason);
    }
}
