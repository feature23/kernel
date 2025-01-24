using F23.Kernel.Results;

namespace F23.Kernel.Tests.Results;

public class AuthorizationResultTests
{
    [Fact]
    public void AuthorizationResult_Success_IsAuthorized()
    {
        // Act
        var result = AuthorizationResult.Success();

        // Assert
        Assert.True(result.IsAuthorized);
        Assert.IsType<SuccessfulAuthorizationResult>(result);
    }

    [Fact]
    public void AuthorizationResult_Fail_IsNotAuthorized()
    {
        // Arrange
        const string message = "YOU SHALL NOT PASS";

        // Act
        var result = AuthorizationResult.Fail(message);

        // Assert
        Assert.False(result.IsAuthorized);
        var failedAuthorizationResult = Assert.IsType<FailedAuthorizationResult>(result);
        Assert.Equal(message, failedAuthorizationResult.Message);
    }
}
