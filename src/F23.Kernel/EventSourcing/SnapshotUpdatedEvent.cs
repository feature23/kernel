namespace F23.Kernel.EventSourcing;

/// <summary>
/// Represents an event that indicates a snapshot has been updated in an event-sourced system.
/// </summary>
/// <typeparam name="T">
/// The type of the aggregate root associated with the snapshot. Must implement <see cref="IAggregateRoot"/>.
/// </typeparam>
/// <remarks>
/// This event is triggered when a snapshot of the state of an aggregate root is updated,
/// typically during the process of event sourcing to optimize data retrieval or system performance.
/// </remarks>
public class SnapshotUpdatedEvent<T> : IDomainEvent
    where T : IAggregateRoot
{
    /// <summary>
    /// Represents the updated snapshot of the aggregate after one or more events was applied.
    /// </summary>
    public required T NewSnapshot { get; init; }

    /// <summary>
    /// A collection of events associated with a snapshot update.
    /// </summary>
    public required IReadOnlyList<IEvent> Events { get; init; }
}
