namespace F23.Kernel.EventSourcing;

public interface IAggregateRootFactory<T, in TCreationEvent>
    where T : IAggregateRoot
    where TCreationEvent : ICreationEvent
{
    Task<Result<T>> Create(TCreationEvent creationEvent, CancellationToken cancellationToken = default);
}
