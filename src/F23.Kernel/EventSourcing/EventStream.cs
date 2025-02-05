namespace F23.Kernel.EventSourcing;

/// <summary>
/// Non-generic static class primarily used for `nameof` operator.
/// </summary>
public static class EventStream;

/// <summary>
/// Represents an event stream for an aggregate root, managing both committed and uncommitted events.
/// </summary>
/// <typeparam name="T">The type of the aggregate root.</typeparam>
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

    /// <summary>
    /// Gets the ID of the snapshot.
    /// </summary>
    public string Id => Snapshot.Id;

    /// <summary>
    /// Gets the list of committed events.
    /// </summary>
    public IReadOnlyList<IEvent> CommittedEvents => _committedEvents;

    /// <summary>
    /// Gets the list of uncommitted events.
    /// </summary>
    public IReadOnlyList<IEvent> UncommittedEvents => _uncommittedEvents;

    /// <summary>
    /// Gets all events, both committed and uncommitted.
    /// </summary>
    public IEnumerable<IEvent> AllEvents => _committedEvents.Concat(_uncommittedEvents);

    /// <summary>
    /// Gets the current snapshot of the aggregate root.
    /// </summary>
    public T Snapshot { get; private set; }

    /// <summary>
    /// Applies an event to the aggregate root.
    /// </summary>
    /// <param name="e">The event to apply.</param>
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

    /// <summary>
    /// Commits all uncommitted events.
    /// </summary>
    public void Commit()
    {
        _committedEvents.AddRange(_uncommittedEvents);
        _uncommittedEvents.Clear();
        _lastCommittedSnapshot = Snapshot;
    }

    /// <summary>
    /// Rolls back all uncommitted events.
    /// </summary>
    public void Rollback()
    {
        Snapshot = _lastCommittedSnapshot;
        _uncommittedEvents.Clear();
    }

    /// <summary>
    /// Gets the last committed snapshot for unit testing purposes.
    ///
    /// SHOULD NOT BE USED IN PRODUCTION CODE.
    /// </summary>
    internal T LastCommittedSnapshot_FOR_UNIT_TESTING => _lastCommittedSnapshot;
}
