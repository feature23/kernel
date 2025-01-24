using F23.Kernel.Results;

namespace F23.Kernel.Tests.Results;

public class PreconditionFailedResultTests
{
    [Theory]
    [InlineData(PreconditionFailedReason.NotFound)]
    [InlineData(PreconditionFailedReason.ConcurrencyMismatch)]
    [InlineData(PreconditionFailedReason.Conflict)]
    public void PreconditionFailedResult_SuccessIsFalse(PreconditionFailedReason reason)
    {
        // Act
        var result = new PreconditionFailedResult(reason, null);

        // Assert
        Assert.False(result.IsSuccess);
    }
}
