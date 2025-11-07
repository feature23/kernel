namespace F23.Kernel.Results;

/// <summary>
/// Represents a result indicating an unauthorized operation.
/// </summary>
public class UnauthorizedResult(string message) : Result(false)
{
    /// <summary>
    /// Gets a message describing the outcome of the operation.
    /// </summary>
    public override string Message => message;
}

/// <summary>
/// Represents a result indicating an unauthorized operation.
/// </summary>
/// <typeparam name="T">The type of the result's associated value, if the operation had succeeded.</typeparam>
public class UnauthorizedResult<T>(string message) : Result<T>(false)
{
    /// <summary>
    /// Gets a message describing the outcome of the operation.
    /// </summary>
    public override string Message => message;

    /// <summary>
    /// Maps to a non-generic unauthorized result.
    /// </summary>
    /// <returns>A non-generic <see cref="UnauthorizedResult"/>.</returns>
    public override Result Map() => Result.Unauthorized(message);
}
