namespace F23.Kernel.Results;

/// <summary>
/// Represents a result where a precondition has failed. This is used to indicate that some condition for proceeding with an
/// operation has not been met, often due to the state of the resource.
/// </summary>
public class PreconditionFailedResult(PreconditionFailedReason reason, string? message) : Result(false)
{
    /// <summary>
    /// Gets the reason why the precondition has failed in a given operation.
    /// </summary>
    /// <remarks>
    /// The value is of type <see cref="PreconditionFailedReason"/> and provides additional context
    /// about the specific reason for the failure, such as <c>NotFound</c>, <c>ConcurrencyMismatch</c>, or <c>Conflict</c>.
    /// </remarks>
    public PreconditionFailedReason Reason { get; } = reason;

    /// <summary>
    /// Gets the message associated with the precondition failure.
    /// If a specific message is provided, it will return that message.
    /// Otherwise, it will return the default message associated with the
    /// <see cref="PreconditionFailedReason"/> value.
    /// </summary>
    public override string Message => message ?? Reason.ToMessage();
}

/// <summary>
/// Represents a result where a precondition has failed. This is used to indicate that some condition for proceeding with an
/// operation has not been met, often due to the state of the resource.
/// </summary>
/// <typeparam name="T">The type of the result's associated value, if the operation had succeeded.</typeparam>
public class PreconditionFailedResult<T>(PreconditionFailedReason reason, string? message) : Result<T>(false)
{
    /// <summary>
    /// Gets the reason why the precondition has failed in a given operation.
    /// </summary>
    /// <remarks>
    /// The value is of type <see cref="PreconditionFailedReason"/> and provides additional context
    /// about the specific reason for the failure, such as <c>NotFound</c>, <c>ConcurrencyMismatch</c>, or <c>Conflict</c>.
    /// </remarks>
    public PreconditionFailedReason Reason { get; } = reason;

    /// <summary>
    /// Gets the message associated with the precondition failure.
    /// If a specific message is provided, it will return that message.
    /// Otherwise, it will return the default message associated with the
    /// <see cref="PreconditionFailedReason"/> value.
    /// </summary>
    public override string Message => message ?? Reason.ToMessage();
}
