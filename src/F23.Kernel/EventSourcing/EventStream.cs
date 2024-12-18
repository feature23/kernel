namespace F23.Kernel.EventSourcing;

/// <summary>
/// Non-generic static class primarily used for `nameof` operator.
/// </summary>
public static class EventStream;

public class EventStream<T> : ISnapshotContainer<T>
    where T : IAggregateRoot
{
    private readonly List<IEvent> _committedEvents = [];
    private readonly List<IEvent> _uncommittedEvents = [];
    private T _lastCommittedSnapshot;

    /// <summary>
    /// Creates an event stream with only a snapshot and no event history.
    /// </summary>
    /// <param name="snapshot">The snapshot of the aggregate root.</param>
    public EventStream(T snapshot)
    {
        Snapshot = snapshot;
        _lastCommittedSnapshot = snapshot;
    }

    /// <summary>
    /// Creates an event stream with a snapshot and a history of events.
    /// </summary>
    /// <param name="snapshot">The snapshot of the aggregate root.</param>
    /// <param name="events">The history of committed events.</param>
    public EventStream(T snapshot, IEnumerable<IEvent> events)
    {
        Snapshot = snapshot;
        _committedEvents.AddRange(events);
        _lastCommittedSnapshot = snapshot;
    }

    public string Id => Snapshot.Id;

    public IReadOnlyList<IEvent> CommittedEvents => _committedEvents;

    public IReadOnlyList<IEvent> UncommittedEvents => _uncommittedEvents;

    public IEnumerable<IEvent> AllEvents => _committedEvents.Concat(_uncommittedEvents);

    public T Snapshot { get; private set; }

    public void Apply(IEvent e)
    {
        if (e is ICreationEvent)
        {
            throw new InvalidOperationException("Cannot apply a creation event after the aggregate root has been created.");
        }

        var applyEventType = typeof(IApplyEvent<,>).MakeGenericType(Snapshot.GetType(), e.GetType());

        if (!applyEventType.IsInstanceOfType(Snapshot))
        {
            throw new InvalidOperationException($"Aggregate root {Snapshot.GetType().Name} does not know how to handle event {e.GetType().Name}");
        }

        var applyMethod = applyEventType.GetMethod(nameof(IApplyEvent<T, IEvent>.Apply))!;

        Snapshot = (T)applyMethod.Invoke(Snapshot, [e])!;
        _uncommittedEvents.Add(e);
    }

    public void Commit()
    {
        _committedEvents.AddRange(_uncommittedEvents);
        _uncommittedEvents.Clear();
        _lastCommittedSnapshot = Snapshot;
    }

    public void Rollback()
    {
        Snapshot = _lastCommittedSnapshot;
        _uncommittedEvents.Clear();
    }
}
