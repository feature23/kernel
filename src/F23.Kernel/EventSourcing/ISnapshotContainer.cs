namespace F23.Kernel.EventSourcing;

public interface ISnapshotContainer<out T>
    where T : IAggregateRoot
{
    T Snapshot { get; }
}
