namespace F23.Kernel.EventSourcing;

public class SnapshotUpdatedEvent<T> : IDomainEvent
    where T : IAggregateRoot
{
    public required T NewSnapshot { get; init; }

    public required IReadOnlyList<IEvent> Events { get; init; }
}
