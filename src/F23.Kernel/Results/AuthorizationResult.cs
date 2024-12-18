namespace F23.Kernel.Results;

public abstract record AuthorizationResult(bool IsAuthorized)
{
    public static AuthorizationResult Success() => new SuccessfulAuthorizationResult();

    public static AuthorizationResult Fail(string message) => new FailedAuthorizationResult(message);
}

public record SuccessfulAuthorizationResult() : AuthorizationResult(true);

public record FailedAuthorizationResult(string Message) : AuthorizationResult(false);