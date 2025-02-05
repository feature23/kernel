namespace F23.Kernel.Results;

/// <summary>
/// Represents the result of an authorization attempt.
/// </summary>
public abstract record AuthorizationResult(bool IsAuthorized)
{
    /// Creates and returns an instance of SuccessfulAuthorizationResult,
    /// representing a successful authorization result.
    /// <returns>An AuthorizationResult indicating success.</returns>
    public static AuthorizationResult Success() => new SuccessfulAuthorizationResult();

    /// Creates a failed authorization result with the specified message.
    /// <param name="message">The failure message associated with the authorization result.</param>
    /// <returns>An instance of FailedAuthorizationResult representing the unsuccessful authorization.</returns>
    public static AuthorizationResult Fail(string message) => new FailedAuthorizationResult(message);
}

/// <summary>
/// Represents a successful authorization result.
/// </summary>
/// <remarks>
/// This class is derived from the <see cref="AuthorizationResult"/> base class and
/// signifies that authorization was successful. The <c>IsAuthorized</c> property
/// is always set to <c>true</c> for instances of this class.
/// </remarks>
public record SuccessfulAuthorizationResult() : AuthorizationResult(true);

/// <summary>
/// Represents the result of a failed authorization attempt.
/// </summary>
/// <remarks>
/// This class is derived from the <see cref="AuthorizationResult"/> base class and
/// signifies that authorization was successful. The <c>IsAuthorized</c> property
/// is always set to <c>false</c> for instances of this class.
/// </remarks>
public record FailedAuthorizationResult(string Message) : AuthorizationResult(false);
