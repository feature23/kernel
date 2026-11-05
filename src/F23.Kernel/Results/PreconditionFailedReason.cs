namespace F23.Kernel.Results;

/// <summary>
/// Defines reasons for a precondition failure in an operation.
/// </summary>
public enum PreconditionFailedReason
{
    /// <summary>
    /// Indicates that a requested resource was not found, leading to the failure of a precondition.
    /// </summary>
    NotFound = 1,

    /// <summary>
    /// Represents a failure due to a concurrency mismatch, typically when the current state of a resource does not align with the expected state.
    /// </summary>
    ConcurrencyMismatch = 2,

    /// <summary>
    /// Indicates that a conflict with the current state of the resource caused the failure of a precondition.
    /// </summary>
    Conflict = 3,

    /// <summary>
    /// Indicates that the operation cannot proceed until the caller supplies an answer to a question it raised,
    /// such as a confirmation or a choice between alternatives.
    /// </summary>
    AnswerRequired = 4,
}

/// <summary>
/// Provides extension methods for the <see cref="PreconditionFailedReason"/> enumeration.
/// </summary>
internal static class PreconditionFailedReasonExtensions
{
    /// <summary>
    /// Converts a <see cref="PreconditionFailedReason"/> to its corresponding descriptive message.
    /// </summary>
    /// <param name="reason">The <see cref="PreconditionFailedReason"/> value to convert.</param>
    /// <returns>A string describing the specified <see cref="PreconditionFailedReason"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the provided <paramref name="reason"/> is not a valid <see cref="PreconditionFailedReason"/> value.
    /// </exception>
    public static string ToMessage(this PreconditionFailedReason reason) => reason switch
    {
        PreconditionFailedReason.NotFound => "A requested resource was not found.",
        PreconditionFailedReason.ConcurrencyMismatch => "A concurrency mismatch occurred.",
        PreconditionFailedReason.Conflict => "A conflict occurred with the current state of the resource.",
        PreconditionFailedReason.AnswerRequired => "The operation needs an answer before it can proceed.",
        _ => throw new ArgumentOutOfRangeException(nameof(reason), reason, null)
    };
}
