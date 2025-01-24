using F23.Kernel.Results;

namespace F23.Kernel.Tests.Results;

public class AggregateResultTests
{
    [Fact]
    public void AggregateResult_AllSuccess_IsSuccess()
    {
        // Arrange
        var results = new List<Result> { Result.Success(), Result.Success() };

        // Act
        var result = new AggregateResult(results);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void AggregateResult_OneFailure_IsFailure()
    {
        // Arrange
        var results = new List<Result> { Result.Success(), Result.Unauthorized("YOU SHALL NOT PASS") };

        // Act
        var result = new AggregateResult(results);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void AggregateResult_EmptyResults_IsSuccess()
    {
        // Act
        var result = new AggregateResult([]);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
