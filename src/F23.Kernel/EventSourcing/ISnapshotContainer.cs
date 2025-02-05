namespace F23.Kernel.EventSourcing;

/// <summary>
/// Defines a container holding a snapshot of an aggregate root in an event-sourced system.
/// </summary>
/// <typeparam name="T">The type of the aggregate root. Must implement <see cref="IAggregateRoot"/>.</typeparam>
public interface ISnapshotContainer<out T>
    where T : IAggregateRoot
{
    /// <summary>
    /// Gets the current snapshot of the aggregate root.
    /// A snapshot represents a specific state of the aggregate, used
    /// to optimize queries.
    /// </summary>
    T Snapshot { get; }
}
