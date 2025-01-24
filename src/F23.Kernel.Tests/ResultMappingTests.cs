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
}
