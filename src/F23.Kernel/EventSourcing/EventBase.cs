using F23.Kernel.Results;

namespace F23.Kernel.EventSourcing;

/// <summary>
/// Represents the base class for all events in the event sourcing system.
/// </summary>
public abstract record EventBase : IEvent
{
    /// <summary>
    /// Gets the type of the event.
    /// </summary>
    public string EventType => GetType().Name;

    /// <summary>
    /// Gets or sets the user profile ID associated with the event.
    /// </summary>
    public string? UserProfileId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the event occurred. Defaults to the current UTC date and time.
    /// </summary>
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Validates the event.
    /// </summary>
    /// <returns>A <see cref="ValidationResult"/> representing the result of the validation.</returns>
    public abstract ValidationResult Validate();
}
