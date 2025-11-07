namespace F23.Kernel.Results;

/// <summary>
/// Represents a successful validation.
/// </summary>
public class ValidationPassedResult() : ValidationResult(true)
{
    /// <summary>
    /// Gets a message describing the outcome of the operation.
    /// </summary>
    public override string Message => "Validation passed";
}

/// <summary>
/// Represents a successful validation.
/// </summary>
/// <typeparam name="T">The type of the associated value.</typeparam>
public class ValidationPassedResult<T>() : ValidationResult<T>(true)
{
    /// <summary>
    /// Gets a message describing the outcome of the operation.
    /// </summary>
    public override string Message => "Validation passed";

    /// <summary>
    /// Maps to a non-generic <see cref="ValidationPassedResult"/> instance.
    /// </summary>
    /// <returns>A non-generic <see cref="ValidationPassedResult"/>.</returns>
    public override Result Map() => new ValidationPassedResult();
}
