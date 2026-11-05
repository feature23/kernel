using System.Net;
using F23.Kernel.Results;

namespace F23.Kernel.AspNetCore;

/// <summary>
/// The HTTP status each <see cref="PreconditionFailedReason"/> maps to, shared by the MVC and Minimal API adapters
/// so that a new reason is mapped in one place.
/// </summary>
internal static class PreconditionFailedReasonHttpExtensions
{
    /// <summary>
    /// The name of the problem details extension member that carries the <see cref="PreconditionFailedReason"/>.
    /// </summary>
    /// <remarks>
    /// Lets a client branch on the reason without parsing the title, which is free text and may be a custom message.
    /// </remarks>
    internal const string ReasonExtension = "reason";

    /// <summary>
    /// Gets the HTTP status code for a precondition failure.
    /// </summary>
    /// <param name="reason">The reason for the precondition failure.</param>
    /// <returns>The HTTP status code to respond with.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="reason"/> is not a defined value.</exception>
    internal static HttpStatusCode ToHttpStatusCode(this PreconditionFailedReason reason) => reason switch
    {
        PreconditionFailedReason.NotFound => HttpStatusCode.NotFound,
        PreconditionFailedReason.ConcurrencyMismatch => HttpStatusCode.PreconditionFailed,
        PreconditionFailedReason.Conflict => HttpStatusCode.Conflict,
        _ => throw new ArgumentOutOfRangeException(nameof(reason), reason, null)
    };
}
