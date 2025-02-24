namespace F23.Kernel.Results;

/// <summary>
/// Represents a successful result of an operation.
/// This class is a specialized implementation of the <see cref="Result"/> type
/// with a predefined success state and message.
/// </summary>
public class SuccessResult() : Result(true)
{
    /// <summary>
    /// Gets a message describing the outcome of the operation.
    /// </summary>
    public override string Message => "The operation was successful.";
}

/// <summary>
/// Represents a successful result of an operation with an associated value.
/// Inherits from the base <see cref="Result{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the associated value.</typeparam>
public class SuccessResult<T>(T value) : Result<T>(true)
{
    /// <summary>
    /// Gets a message describing the outcome of the operation.
    /// </summary>
    public override string Message => "The operation was successful.";

    /// <summary>
    /// Gets the value of the result.
    /// This represents the successful outcome of the operation when the result is a <see cref="SuccessResult{T}"/>.
    /// </summary>
    public T Value => value;
}
