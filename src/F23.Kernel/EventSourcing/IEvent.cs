namespace F23.Kernel.EventSourcing;

/// <summary>
/// Represents a domain event that is part of an event-sourced system.
/// </summary>
/// <remarks>
/// This interface combines the functionality of a domain event marker with the ability to validate itself.
/// It also provides metadata about the event.
/// Implementors should define specific event types and their associated data.
/// </remarks>
public interface IEvent : IDomainEvent, IValidatable
{
    /// <summary>
    /// Gets the type of the event as a string representation.
    /// </summary>
    /// <remarks>
    /// This property is used to identify the specific event type associated with the implementation.
    /// The value is generally set as the name of the implementing class or a predefined string.
    /// </remarks>
    string EventType { get; }

    /// <summary>
    /// Gets or sets the identifier of the user profile associated with the event.
    /// </summary>
    /// <remarks>
    /// This property links an event to a specific user profile, enabling tracking or filtering of events
    /// related to a particular user. It is nullable to support scenarios where the event is not
    /// raised by a user.
    /// </remarks>
    string? UserProfileId { get; set; }

    /// <summary>
    /// Gets or sets the date and time at which the event occurred.
    /// </summary>
    /// <remarks>
    /// This property is typically used to indicate when the event was raised
    /// or recorded within an event sourcing system. The value is expected
    /// to be in UTC format.
    /// </remarks>
    DateTime OccurredAt { get; set; }
}
