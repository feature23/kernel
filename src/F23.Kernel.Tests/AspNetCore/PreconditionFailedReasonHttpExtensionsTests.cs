using System.Net;
using F23.Kernel.AspNetCore;
using F23.Kernel.Results;
using F23.Kernel.Tests.Mocks;

namespace F23.Kernel.Tests.AspNetCore;

public class PreconditionFailedReasonHttpExtensionsTests
{
    [Theory]
    [MemberData(nameof(PreconditionFailedReasons.All), MemberType = typeof(PreconditionFailedReasons))]
    public void EveryReason_HasAStatusCode(PreconditionFailedReason reason)
    {
        // Act
        var statusCode = reason.ToHttpStatusCode();

        // Assert
        Assert.InRange((int)statusCode, 400, 499);
    }

    [Theory]
    [InlineData(PreconditionFailedReason.NotFound, HttpStatusCode.NotFound)]
    [InlineData(PreconditionFailedReason.ConcurrencyMismatch, HttpStatusCode.PreconditionFailed)]
    [InlineData(PreconditionFailedReason.Conflict, HttpStatusCode.Conflict)]
    public void Reason_MapsToItsStatusCode(PreconditionFailedReason reason, HttpStatusCode expected)
    {
        Assert.Equal(expected, reason.ToHttpStatusCode());
    }

    [Fact]
    public void UndefinedReason_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ((PreconditionFailedReason)999).ToHttpStatusCode());
    }
}
