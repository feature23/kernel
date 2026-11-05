using F23.Kernel.Results;
using F23.Kernel.Tests.Mocks;

namespace F23.Kernel.Tests.Results;

public class PreconditionFailedResultTests
{
    [Theory]
    [MemberData(nameof(PreconditionFailedReasons.All), MemberType = typeof(PreconditionFailedReasons))]
    public void PreconditionFailedResult_SuccessIsFalse(PreconditionFailedReason reason)
    {
        // Act
        var result = new PreconditionFailedResult(reason, null);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Theory]
    [MemberData(nameof(PreconditionFailedReasons.All), MemberType = typeof(PreconditionFailedReasons))]
    public void MappingPreconditionFailedResultToOther_PreservesTheReasonAndMessage(PreconditionFailedReason reason)
    {
        // Arrange
        var originalMessage = "Original failure message";
        var originalResult = new PreconditionFailedResult<Original>(reason, originalMessage);

        // Act
        var mappedResult = originalResult.MapFailure<Other>();

        // Assert
        var preconditionFailedResult = Assert.IsType<PreconditionFailedResult<Other>>(mappedResult);
        Assert.Equal(reason, preconditionFailedResult.Reason);
        Assert.Equal(originalMessage, preconditionFailedResult.Message);
    }

    [Theory]
    [MemberData(nameof(PreconditionFailedReasons.All), MemberType = typeof(PreconditionFailedReasons))]
    public void PreconditionFailedResult_WithoutMessage_HasADefaultMessageForEveryReason(PreconditionFailedReason reason)
    {
        // Act
        var result = new PreconditionFailedResult(reason, null);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(result.Message));
    }

    private class Original { }

    private class Other { }
}
